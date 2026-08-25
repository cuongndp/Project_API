namespace DataAccess.Models
{
    public class dh_User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Avatar { get; set; }
    }
    public class Request_dh_User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
    }
    public class Request_Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? CreatedByIp { get; set; }
        public string? UserAgent { get; set; }
        public string? DeviceID { get; set; }
    }
    public class Return_request
    {
        public bool Seccess { get; set; }
        public string Message { get; set; }
    }
    public class ResponseLogin
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public bool Seccess { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
    }
}
