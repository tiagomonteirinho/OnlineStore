using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DataContext context) : base(context)
        {
            // No more code needed due to using Repository design pattern.
        }
    }
}
