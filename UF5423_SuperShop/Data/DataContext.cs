using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Data
{
    //public class DataContext : DbContext // 'DbContext': basic context without security.
    public class DataContext : IdentityDbContext<User> // 'IdentityDbContext': context with user authentication.
    {
        public DbSet<Product> Products { get; set; } // Property which defines a data table.

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderDetailTemp> OrderDetailsTemp { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
    }
}
