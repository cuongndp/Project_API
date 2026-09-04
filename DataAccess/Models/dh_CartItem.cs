using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class dh_CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        public int CartID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
    public class Request_CartItem
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
    public class CartItemViewModel
    {
        public int CartItemID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }
        public decimal? VAT { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public string? Image { get; set; }
    }
    
}