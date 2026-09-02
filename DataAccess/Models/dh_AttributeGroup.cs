using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class dh_AttributeGroup
    {
        [Key]
        public int GroupID { get; set; }

        public string Name { get; set; }

        public int? Sort { get; set; }
    }
}
