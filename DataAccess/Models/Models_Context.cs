using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models
{
    public class Models_Context : DbContext
    {
        public Models_Context(DbContextOptions<Models_Context> options) : base(options) { }
        public DbSet<dh_User> dh_User { get; set; }
        public DbSet<dh_UserSession> dh_UserSession { get; set; }
        public DbSet<dh_Brand> dh_Brand { get; set; }
        public DbSet<dh_Product> dh_Product { get; set; }
        public DbSet<dh_ProductCategory> dh_ProductCategorie { get; set; }
        public DbSet<dh_ProductImage> dh_ProductImage  { get; set; }
        public DbSet<dh_Attribute> dh_Attribute { get; set; }
        public DbSet<dh_AttributeGroup> dh_AttributeGroup { get; set; } 
        public DbSet<dh_ProductAttribute>  dh_ProductAttribute { get; set; }
    }
}
