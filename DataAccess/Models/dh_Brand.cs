using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class dh_Brand
    {
        [Key]
        public int BrandID { get; set; }

        public string? Name { get; set; }
    }
}
