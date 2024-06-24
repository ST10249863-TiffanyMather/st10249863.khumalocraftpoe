using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KhumaloCraft_Part2.Models;

namespace KhumaloCraft_Part2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<KhumaloCraft_Part2.Models.User> User { get; set; } = default!;
        public DbSet<KhumaloCraft_Part2.Models.Product> Product { get; set; } = default!;
        public DbSet<KhumaloCraft_Part2.Models.Transaction> Transaction { get; set; } = default!;
        public DbSet<KhumaloCraft_Part2.Models.ProductTransaction> ProductTransaction { get; set; } = default!;
  
    }
}
