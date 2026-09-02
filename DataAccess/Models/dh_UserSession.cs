using Microsoft.AspNetCore.Server.HttpSys;

namespace DataAccess.Models
{
    public class dh_UserSession
    {
          public int ID { get; set; }
          public int  UserID    {get;set;}
          public string  RefreshToken  {get;set;}
          public DateTime  CreateDate {get;set;}
          public DateTime  ExpiryDate   {get;set;}
          public bool  IsUser  {get;set;}
          public bool  IsRevoked    {get;set;}
          public string  CreatedByIp    {get;set;}
          public string  UserAgent    {get;set;}
       public string DeviceID {get;set;}
        public string?  ReplacedByToken            {get;set;}
          public string?  RevokedByIp                {get;set;}
          public string? ReasonForRevocation        {get;set;}
        public string SessionId {get;set;}
    }
    
    
    public class Request_Refresh
    {
        public string CreatedByIp { get; set; }
        public string UserAgent { get; set; }
        public string RefreshToken { get; set; }
        public string DeviceID { get; set; }
    }
  
   
   

    

}
