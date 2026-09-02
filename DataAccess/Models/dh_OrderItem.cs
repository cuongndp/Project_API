using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class dh_OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public decimal? VAT { get; set; }

        public int Quantity { get; set; }
    }
}