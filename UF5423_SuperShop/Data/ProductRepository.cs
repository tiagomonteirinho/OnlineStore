using Microsoft.EntityFrameworkCore;
using System.Linq;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository // 'IProductRepository' for instantiation at Startup.cs where dependency injection requires interface before class.
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUser() // Get data from different table.
        {
            return _context.Products.Include(p => p.User);
        }
    }
}
