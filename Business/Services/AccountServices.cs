using Business.IServices;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
namespace Business.Services
{
    public class AccountServices : IAccountServices
    {
        private Models_Context _context;
        private ITokenServices _tokenServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;
        public AccountServices(Models_Context context, ITokenServices tokenServices, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        {
            _context = context;
            _tokenServices = tokenServices;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Return_request> Regiter(Request_dh_User request)
        {
            if (request.Email != null && request.Password != null && request.Address != null && request.FullName != null && request.Name != null)
            {
                var user = await _context.dh_User.FirstOrDefaultAsync(x=>x.Email==request.Email);
                if (user != null) return new Return_request
                {
                    Seccess = false,
                    Message = "Email đã tồn tại"
                };
                else
                {
                    var hashPass = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    var newUser = new dh_User
                    {
                        Email = request.Email,
                        Password = hashPass, 
                        Name = request.Name, 
                        FullName = request.FullName, 
                        Address = request.Address
                        
                    };
                    _context.dh_User.Add(newUser);
                   await _context.SaveChangesAsync();
                    return new Return_request
                    {
                        Seccess = true,
                        Message = "Đăng ký thành công"
                    };
                } 
                    
            }
            return new Return_request
            {
                Seccess = false,
                Message = "Vui lòng điền đầy đủ thông tin"
            };
        }
        public async Task<ResponseLogin> Login(Request_Login request_Login)
        {
            try
            {
                var user = await _context.dh_User.FirstOrDefaultAsync(x => x.Email == request_Login.Email);
                if(user != null)
                {
                    var pass = BCrypt.Net.BCrypt.Verify(request_Login.Password, user.Password);

                    // check thiết bị
                    
                    if(pass)
                    {
                        var session = await _context.dh_UserSession.FirstOrDefaultAsync(x => x.UserID == user.ID && x.IsRevoked == false);
                        if (session != null)
                        {
                            session.IsRevoked = true;
                        }


                       
                        var refreshToken = _tokenServices.GenerateRefreshToken();
                        var day = Convert.ToInt32(_configuration["jwt:RefreshTokenValidityInDays"]);
                        var expiryDate = DateTime.UtcNow.AddDays(day);
                        var newUser = new dh_UserSession
                        {
                            RefreshToken = refreshToken,
                            CreateDate = DateTime.UtcNow,
                            ExpiryDate = expiryDate,
                            UserID = user.ID,
                            CreatedByIp = request_Login.CreatedByIp,
                            UserAgent = request_Login.UserAgent,
                            DeviceID = request_Login.DeviceID


                        };
                        await _context.dh_UserSession.AddAsync(newUser);
                        await _context.SaveChangesAsync();

                        
                        var authClaim = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.Name),
                            new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),
                            new Claim("SessionId",newUser.ID.ToString())
                        };
                        var token = _tokenServices.GenerateAccessToken(authClaim);
                        var refreshTokenOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Lax, // Đổi từ None thành Lax để chống CSRF nếu có thể
                            Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["Jwt:RefreshTokenValidityInDays"])),

                        };
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, refreshTokenOptions);
                        return new ResponseLogin
                            {
                                UserName = user.Name,
                                Email = request_Login.Email,
                                AccessToken = token,
                                Seccess = true,
                                Message="Đăng nhập thành công",

                            };
                        
                    }
                    return new ResponseLogin
                    {
                        Seccess = false,
                        Message = "Đăng nhập thất bại",

                    };
                }
                return new ResponseLogin
                {
                    Seccess = false,
                    Message = "Đăng nhập thất bại",

                };

            }
            catch (Exception ex) 
            {
                throw ex;
            }
            
        }
        public async Task Logout(string refresh)
        {
            var session = await _context.dh_UserSession.FirstOrDefaultAsync(x => x.RefreshToken == refresh && x.IsRevoked==false);
            if (session != null)
            {
                session.IsRevoked = true;
                _context.dh_UserSession.Update(session);
                await _context.SaveChangesAsync();
            } 
                
        }

        public async Task<Return_request> Post_Avatar(string id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new Return_request
                {
                    Seccess = false,
                    Message = "File không hợp lệ"
                };
            } 
            var duoi= Path.GetExtension(file.FileName);  //lấy đuôi vd jpg

            var fileName = $"{Guid.NewGuid()}{duoi}";
            var folder = Path.Combine(
                Directory.GetCurrentDirectory(),  // dòng này lấy ra tên project vd Project_API/Application/wwwroot/Avatar
                "wwwroot",
                "Avatars"
                );
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);  //nếu ko có folder thì tạo 

            var filePath =Path.Combine(folder, fileName);  //ghép đường dẫn với tên fide Project_API/Business/Avatar/tenfile.jpg

            using (var stream = new FileStream(filePath, FileMode.Create))  // tạo ra 1 cái nói chung ghi file vào nếu trùng thì ghi đè còn ko trùng thì tạo mới với đường dẫn fide đó là filePath
            {
                await file.CopyToAsync(stream);
            }
            var user = await _context.dh_User.FirstOrDefaultAsync(x => x.ID == Convert.ToInt32(id));
            if ( user== null)
            {
                return new Return_request
                {
                    Seccess = false,
                    Message = "error"
                };
            }
            var oldAvatar = Path.GetFileName(user.Avatar);
            user.Avatar = $"/Avatars/{fileName}";
            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(oldAvatar))
            {
                
                var oldfilePath=Path.Combine(folder, oldAvatar);
                if(File.Exists(oldfilePath))
                    File.Delete(oldfilePath);
            } 
                
           
            return new Return_request
            {
                Seccess = true,
                Message = "Cập nhật thành công"
            };
        }
    }
}
