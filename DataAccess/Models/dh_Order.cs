using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class dh_Order
    {
        [Key]
        public int OrderID { get; set; }

        public int UserID { get; set; }

        public string OrderCode { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }

        public decimal ShippingFee { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal VATAmount { get; set; }

        public decimal TotalAmount { get; set; }

        // OrderStatus:
        // 0 = Pending      - Chờ xác nhận
        // 1 = Confirmed    - Đã xác nhận
        // 2 = Preparing    - Đang chuẩn bị hàng
        // 3 = Shipping     - Đang giao
        // 4 = Delivered    - Đã giao
        // 5 = Cancelled    - Đã hủy
        // 6 = Returned     - Đã trả hàng
        public byte OrderStatus { get; set; }

        public string? PaymentMethod { get; set; }

        // PaymentStatus:
        // 0 = Unpaid       - Chưa thanh toán
        // 1 = Paid         - Đã thanh toán
        // 2 = Failed       - Thanh toán thất bại
        // 3 = Refunded     - Đã hoàn tiền
        public byte PaymentStatus { get; set; }

        public string ShippingName { get; set; } = string.Empty;

        public string ShippingPhone { get; set; } = string.Empty;

        public string ShippingAddress { get; set; } = string.Empty;

        public string? Note { get; set; }

        public string? CancelReason { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}