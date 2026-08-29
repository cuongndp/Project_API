namespace DataAccess.Models
{
    public class dh_ProductImage
    {
        public int ProductImageID { get; set; }

        public int? ProductID { get; set; }

        public string? ImageUrl { get; set; }

        public int? Sort { get; set; }
    }
}
