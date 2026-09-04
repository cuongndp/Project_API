using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class dh_ProductAttribute
    {
        [Key]
        public int ProductAttributeID { get; set; }

        public int ProductID { get; set; }

        public int AttributeID { get; set; }

        public string? Value { get; set; }

        public int? Sort { get; set; }
    }
    public class ProductDetailViewModel
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public int Quantity { get; set; }

        public int Warranty { get; set; }

        public bool Hot { get; set; }
        public decimal? VAT { get; set; }

        public string Description { get; set; }

        public string Detail { get; set; }

        public string CategoryName { get; set; }

        public string BrandName { get; set; }

        public string MainImage { get; set; }

        public List<string> Images { get; set; }

        public List<ProductAttributeViewModel> Attributes { get; set; }
    }
    public class ProductAttributeViewModel
    {
        public string GroupName { get; set; }

        public string AttributeName { get; set; }

        public string AttributeValue { get; set; }

        public int Sort { get; set; }
    }
}
