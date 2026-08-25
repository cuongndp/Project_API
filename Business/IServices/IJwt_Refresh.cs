using DataAccess.Models;

namespace Business.IServices
{
    public interface IJwt_Refresh
    {

        Task<string> Refresh_Token(Request_Refresh request);
    }
}
