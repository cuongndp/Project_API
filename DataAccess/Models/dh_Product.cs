using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class dh_Product
    {
        [Key]
        public int ProductID { get; set; }

        public string? ProductName { get; set; }

        public string? SeoTitle { get; set; }

        public bool? Status { get; set; }

        public string? Image { get; set; }

        public decimal? Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public decimal? VAT { get; set; }

        public int? Quantity { get; set; }

        public int? Warranty { get; set; }

        public bool? Hot { get; set; }

        public string? Desription { get; set; }

        public string? Detail { get; set; }

        public int? ViewCount { get; set; }

        public int? CateID { get; set; }

        public int? BrandID { get; set; }

        public int? SupplierID { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescriptions { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
