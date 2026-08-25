using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models
{
    public class Models_Context:DbContext
    {
        public Models_Context(DbContextOptions<Models_Context> options):base(options) { }
        public DbSet<dh_User> dh_User { get; set; }
        public DbSet<dh_UserSession> dh_UserSession { get; set; }
    }
}
