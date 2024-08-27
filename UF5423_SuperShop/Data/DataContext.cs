using Microsoft.EntityFrameworkCore;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; } // property which defines a data table.

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
    }
}
