using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Data
{
    //public class DataContext : DbContext // 'DbContext': basic context without security.
    public class DataContext : IdentityDbContext<User> // 'IdentityDbContext': ASP.NET built-in database context with authentication.
    {
        public DbSet<Product> Products { get; set; } // Property which defines a data table.

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderDetailTemp> OrderDetailsTemp { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder) // Cascade deletion rule. // Requires DB creation to be set.
        //{
        //    var cascadeFKs = modelBuilder.Model // From model
        //        .GetEntityTypes() // Get all tables
        //        .SelectMany(t => t.GetForeignKeys()) // Select all foreign keys
        //        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        //    foreach (var fk in cascadeFKs)
        //    {
        //        fk.DeleteBehavior = DeleteBehavior.Restrict; // Prevent deletion.
        //    }

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
