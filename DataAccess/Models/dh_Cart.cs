using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class dh_Cart
    {
        [Key]
        public int CartID { get; set; }

        public int UserID { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}