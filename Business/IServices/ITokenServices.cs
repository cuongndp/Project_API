using DataAccess.Models;
using System.Security.Claims;

namespace Business.IServices
{
    public interface ITokenServices
    {
        public string GenerateRefreshToken(); //tạo refresh token
        public string GenerateAccessToken(IEnumerable<Claim> claims); //tạo token
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token); //giải mã token
        

    }
}
