using Azure.Core;
using Business.IServices;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        private IAccountServices _accountServices;
        private IJwt_Refresh _jwt_Refresh;

        public HomeController(IAccountServices accountServices, IJwt_Refresh jwt_Refresh)
        {
            _accountServices = accountServices;
            _jwt_Refresh = jwt_Refresh;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(Request_dh_User request)
        {
            var req = await _accountServices.Regiter(request);
            return Ok(req);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Request_Login request_Login)
        {
            request_Login.CreatedByIp = HttpContext.Connection.RemoteIpAddress.ToString();
            request_Login.UserAgent = Request.Headers["User-Agent"].ToString();
            request_Login.DeviceID = HttpContext.Items["DeviceID"]?.ToString();
            if (request_Login.CreatedByIp != null && request_Login.UserAgent != null)
            {
                var check = await _accountServices.Login(request_Login);
                return Ok(check);
            }
            return BadRequest();
        }
        [HttpPost("Refresh_Token")]
        public async Task<IActionResult> Refresh_Token()
        {
            var ip = new Request_Refresh
            {
                RefreshToken = HttpContext.Request.Cookies["refreshToken"],
                DeviceID = HttpContext.Request.Cookies["DeviceID"],
                UserAgent = Request.Headers["User-Agent"].ToString(),
                CreatedByIp = HttpContext.Connection.RemoteIpAddress.ToString()

            };
            var token = await _jwt_Refresh.Refresh_Token(ip);
            if (token != null)
                return Ok(token);
            HttpContext.Response.Cookies.Delete("refreshToken");
            return Unauthorized(new
            {
                Success = false,
                Message = "Refresh Token không hợp lệ hoặc đã hết hạn"
            });
        }
        [HttpPost("Logout")]
        public async Task<IActionResult>Logout()
        {
            var refresh_Token = HttpContext.Request.Cookies["refreshToken"];
            await _accountServices.Logout(refresh_Token);
            HttpContext.Response.Cookies.Delete("refreshToken");
            return Ok();           
        }
        [Authorize]
        [HttpPost("Post_Avatar")]
        public async Task<IActionResult> Post_Avatar(IFormFile file)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var req= await _accountServices.Post_Avatar(userID, file);
            return Ok(req);
        }

    }
}
