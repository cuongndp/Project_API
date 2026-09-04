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
    public class Request_CheckoutCart
    {
        public List<int> CartItemIDs { get; set; } 
    }
    public class Request_CheckoutBuyNow
    {
        public int ProductID { get; set; }

        public int Quantity { get; set; }
    }
    public class CheckoutPreviewResponse
    {
        public List<CheckoutItemViewModel> Items { get; set; } 

        public decimal Subtotal { get; set; }

        public decimal ShippingFee { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal VATAmount { get; set; }

        public decimal TotalAmount { get; set; }
    }
    public class CheckoutItemViewModel
    {
        public int CartItemID { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string? Image { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}