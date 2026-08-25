using Business.IServices;
using DataAccess.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Business.Services
{
    

    public class TokenServices :  ITokenServices
    {

        private readonly Models_Context _context;
        private readonly IConfiguration _configuration;
        public TokenServices(IConfiguration configuration, Models_Context context)
        {
            _configuration = configuration;
            _context = context;
        }
        public string GenerateRefreshToken()  //tạo refresh token
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }

        public string GenerateAccessToken(IEnumerable<Claim> claims) //truyền claim dô để tạo accesstoken
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var minute = Convert.ToInt32(_configuration["Jwt:TokenValidityInMinutes"]);
            var jwtClaims = claims?.ToList() ?? new List<Claim>();
            if (!jwtClaims.Any(c => c.Type == JwtRegisteredClaimNames.Jti))
            {
                jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            }
            var jwt = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                claims: jwtClaims, //the user's claims, for example new Claim[] { new Claim(ClaimTypes.Name, "The username"), //... 
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(minute),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt); //the method is called WriteToken but returns a string
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }


        ////bên ngoài sẽ truyền vào 2 tham số là refreshtoken và deviceid
        //public async Task<ServiceResult<string>> RefreshToken(RefreshTokenRequest token)
        //{
            
        //    if (string.IsNullOrEmpty(token.RefreshToken) || string.IsNullOrEmpty(token.DeviceID))
        //    {
                
        //        return new ServiceResult<string>
        //        {
        //            Success = false,
        //            ErrorCode = "AUTH_MISSING_COOKIES",
        //            Message = "Phiên làm việc không hợp lệ do thiếu thông tin xác thực thiết bị."
        //        };
        //    }
        //    var user_Session =await _context.dh_UserSession.FirstOrDefaultAsync(x=>x.RefreshToken== token.RefreshToken&&x.DeviceID==token.DeviceID);
        //    if (user_Session == null)
        //    {
        //        // này là câu để xóa token 
        //        _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken",
        //             new CookieOptions
        //             {
        //                 Secure = true,
        //                 SameSite = SameSiteMode.None,
        //                 Path = "/"
        //             });
        //        return new ServiceResult<string>
        //        {
        //            Success = false,
        //            ErrorCode = "AUTH_TOKEN_ERROR",
        //            Message = "Đã có lỗi xảy ra vui lòng đăng nhập lại."
        //        };
        //    }
        //    if (user_Session.IsUser == true || user_Session.IsRevoked == true)
        //    {
        //        var khoa = new DynamicParameters();
        //        khoa.Add("@p_UserID", user_Session.UserID);
        //        khoa.Add("@p_ReasonForRevocation", "Phát hiện đăng nhập bất thường tiến hành đăng xuất tất cả thiết bị vào lúc:" + DateTime.UtcNow);
        //        await connection.ExcuteAsync("sp_IsRevokedIsUser", khoa);
        //        // này là câu để xóa token 
        //        _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken",
        //             new CookieOptions
        //             {
        //                 Secure = true,
        //                 SameSite = SameSiteMode.None,
        //                 Path = "/"
        //             });
        //        return new ServiceResult<string>
        //        {
        //            Success = false,
        //            ErrorCode = "AUTH_TOKEN_ISUSER",
        //            Message = "Phát hiện đăng nhập bất thường tiến hành đăng xuất tất cả thiết bị vui lòng đăng nhập lại"
        //        };
        //    }

        //    if (user_Session.ExpiryDate < DateTime.UtcNow)
        //    {
        //        // này là câu để xóa token 
        //        _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken",
        //             new CookieOptions
        //             {
        //                 Secure = true,
        //                 SameSite = SameSiteMode.None,
        //                 Path = "/"
        //             });
        //        return new ServiceResult<string>
        //        {
        //            Success = false,
        //            ErrorCode = "AUTH_TOKEN_EXPIRED",
        //            Message = "Phiên đăng nhập của bạn đã hết hạn. Vui lòng đăng nhập lại."
        //        };
        //    }

        //    // qua ải và bắt đầu tạo token mới
        //    var newRefreshToken = GenerateRefreshToken();
        //    var name=await _context.dh_User.Where(x=>x.ID==user_Session.UserID)
        //        .Select(x=> new
        //        {
        //            x.Name,
        //            x.IsActive
        //        }).FirstOrDefaultAsync();
        //    if (name == null)
        //    {
        //        // này là câu để xóa token 
        //        _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken",
        //             new CookieOptions
        //             {
        //                 Secure = true,
        //                 SameSite = SameSiteMode.None,
        //                 Path = "/"
        //             });

        //        return new ServiceResult<string>
        //        {
        //            Success = false,
        //            ErrorCode = "USER_NOT_FOUND",
        //            Message = "Người dùng không tồn tại."
        //        };
        //    }
        //    if (!name.IsActive)
        //    {
        //        // này là câu để xóa token 
        //        _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken",
        //             new CookieOptions
        //             {
        //                 Secure = true,
        //                 SameSite = SameSiteMode.None,
        //                 Path = "/"
        //             });

        //        return new ServiceResult<string>
        //        {
        //            Success = false,
        //            ErrorCode = "USER_LOCKED",
        //            Message = "Tài khoản đã bị khóa."
        //        };
        //    }
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    var authClaim = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Name,name.Name),
        //                new Claim(ClaimTypes.NameIdentifier,user_Session.UserID.ToString()),
        //            };
        //    var accessTokenNew =GenerateAccessToken(authClaim);
            
        //    user_Session.ReplacedByToken = newRefreshToken;
        //    user_Session.IsUser = true;

        //    var expiryDate = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["Jwt:RefreshTokenValidityInDays"]));

        //    var newUserSession = new dh_UserSession
        //    {
        //        UserID = user_Session.UserID,
        //        RefreshToken = newRefreshToken,
        //        ExpiryDate = expiryDate,
        //        CreatedByIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString(),
        //        UserAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString(),
        //        CreateDate=DateTime.UtcNow,
        //        DeviceID = token.DeviceID
        //    };
        //    // await _context.dh_UserSession.AddAsync(newUserSession);

        //    //await _context.SaveChangesAsync();


           
        //    try
        //    {
        //        // cập nhật token cũ
               
        //        _context.dh_UserSession.Update(user_Session);
        //        // thêm token mới
        //        await _context.dh_UserSession.AddAsync(newUserSession);

        //        // lưu
        //        await _context.SaveChangesAsync();

        //        // xác nhận
               


        //        var refreshTokenOptions = new CookieOptions
        //        {
        //            HttpOnly = true,
        //            Secure = true,
        //            SameSite = SameSiteMode.None, // Đổi từ None thành Lax để chống CSRF nếu có thể
        //            Path = "/",
        //            Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["Jwt:RefreshTokenValidityInDays"]))
        //        };
        //        _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken, refreshTokenOptions);
        //        await transaction.CommitAsync();
        //    }
        //    catch (Exception)
        //    {
        //        await transaction.RollbackAsync();

        //        _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken",
        //            new CookieOptions
        //            {
        //                Secure = true,
        //                SameSite = SameSiteMode.None,
        //                Path = "/"
        //            });

        //        return new ServiceResult<string>
        //        {
        //            Success = false,
        //            ErrorCode = "SERVER_ERROR",
        //            Message = "Có lỗi xảy ra, vui lòng đăng nhập lại."
        //        };
        //    }

           
           
        //    return new ServiceResult<string>
        //    {
        //        Success = true,
        //        ErrorCode = "SUCCESS",
        //        Message = "success.",
        //        Token=accessTokenNew
        //    };

        //}



    }

}
