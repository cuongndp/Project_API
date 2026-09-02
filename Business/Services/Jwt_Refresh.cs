using Business.IServices;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace Business.Services
{
    public class Jwt_Refresh : IJwt_Refresh
    {
        private Models_Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ITokenServices _tokenServices;
        private IConfiguration _configuration;
        public IDistributedCache _cache;

        public Jwt_Refresh(IDistributedCache cache,Models_Context context , ITokenServices tokenServices, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _tokenServices = tokenServices;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        public async Task<string> Refresh_Token(Request_Refresh request)
        {
            var checkToken= await _context.dh_UserSession.FirstOrDefaultAsync (x=>
            
                x.RefreshToken==request.RefreshToken       && x.IsRevoked == false 
            );
            if (checkToken!=null&&(
                checkToken.ExpiryDate < DateTime.UtcNow ||
                checkToken.DeviceID != request.DeviceID))
            {
                checkToken.IsRevoked = true;
                checkToken.ReasonForRevocation = "Token đã hết hạn hoặc phát hiện truy cập thiết bị khác";
                 _context.dh_UserSession.Update(checkToken);
                await _context.SaveChangesAsync();
                return null;
            }    
                

            if (checkToken != null)
            {
                var user = await _context.dh_User.FirstOrDefaultAsync(x => x.ID == checkToken.UserID);
                
                var authClaim = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.Name),
                            new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),
                            new Claim("SessionId",checkToken.SessionId)
                        };
                var token = _tokenServices.GenerateAccessToken(authClaim);
                // bắt đầu tạo key và lưu token vào redis

                var TokenKey = $"user:{user.ID}:active_session";
                var cacheData = await _cache.GetStringAsync(TokenKey);

                //nếu có xóa cái token cũ và tạo cái mới cho thiết bị mới đó
                //if(!string.IsNullOrEmpty(cacheData))
                //{
                //    await _cache.RemoveAsync(TokenKey);
                //}    


                await _cache.SetStringAsync(TokenKey, checkToken.SessionId, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Convert.ToInt32(_configuration["Jwt:TokenValidityInMinutes"]))
                });
                var refreshToken = _tokenServices.GenerateRefreshToken();
                var day = Convert.ToInt32(_configuration["jwt:RefreshTokenValidityInDays"]);
                var expiryDate = DateTime.UtcNow.AddDays(day);
                var newUser = new dh_UserSession
                {
                    RefreshToken = refreshToken,
                    CreateDate = DateTime.UtcNow,
                    ExpiryDate = expiryDate,
                    UserID = user.ID,
                    CreatedByIp = request.CreatedByIp,
                    UserAgent = request.UserAgent,
                    DeviceID = request.DeviceID,
                    SessionId=checkToken.SessionId


                };
                checkToken.ReplacedByToken = refreshToken;
                checkToken.RevokedByIp=request.CreatedByIp;
                checkToken.IsRevoked = true;
                var refreshTokenOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax, 
                    Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["Jwt:RefreshTokenValidityInDays"])),

                };
                _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, refreshTokenOptions);
                _context.dh_UserSession.Update(checkToken);
                await _context.dh_UserSession.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return token;
            }
            return null;
                
        }
    }
}
