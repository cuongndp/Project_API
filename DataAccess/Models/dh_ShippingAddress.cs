using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class dh_ShippingAddress
    {
        [Key]
        public int ShippingAddressID { get; set; }

        public int UserID { get; set; }

        public string RecipientName { get; set; } 

        public string RecipientPhone { get; set; }

        public string Address { get; set; } 
    }
    public class Request_ShippingAddress
    {       

        public string RecipientName { get; set; }

        public string RecipientPhone { get; set; }

        public string Address { get; set; }
    }
}
