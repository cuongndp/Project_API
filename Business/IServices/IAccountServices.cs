using DataAccess.Models;

namespace Business.IServices
{
    public interface IAccountServices
    {
        Task<Return_request> Regiter(Request_dh_User request);
        Task<ResponseLogin> Login (Request_Login request_Login);
        Task Logout(string refresh);
        Task<Return_request> Post_Avatar(string id,IFormFile file);
    }
}
