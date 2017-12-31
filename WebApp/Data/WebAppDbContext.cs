using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;

namespace WebApp.Data
{
    public class WebAppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("WebAppDatabase");
        }
    }
}