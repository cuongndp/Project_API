namespace DataAccess.Models
{
    public class dh_ProductCategory
    {
        public int CateID { get; set; }

        public string? Name { get; set; }

        public string? SeoTitle { get; set; }

        public bool? Status { get; set; }

        public int? Sort { get; set; }

        public int? ParentID { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescriptions { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
    public class ProductViewModel
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public int Quantity { get; set; }

        public int Warranty { get; set; }

        public bool Hot { get; set; }

        public string Desription { get; set; }

        public string Detail { get; set; }

        public string CategoryName { get; set; }

        public string BrandName { get; set; }

        public string MainImage { get; set; }

    }

}
