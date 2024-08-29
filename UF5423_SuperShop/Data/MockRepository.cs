using System.Collections.Generic;
using System.Threading.Tasks;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Data
{
    public class MockRepository : IRepository // Repository mock file for quick testing. // Replace IServiceCollection service in Startup file to enable.
    {
        public void AddProduct(Product product)
        {
            throw new System.NotImplementedException();
        }

        public Product GetProduct(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Product> GetProducts()
        {
            var products = new List<Product>();
            products.Add(new Product { ProductId = 1, ProductName = "TestItemName1", ProductPrice = 10 });
            products.Add(new Product { ProductId = 2, ProductName = "TestItemName2", ProductPrice = 20 });
            products.Add(new Product { ProductId = 3, ProductName = "TestItemName3", ProductPrice = 30 });
            products.Add(new Product { ProductId = 4, ProductName = "TestItemName4", ProductPrice = 40 });
            products.Add(new Product { ProductId = 5, ProductName = "TestItemName5", ProductPrice = 50 });

            return products;
        }

        public bool ProductExists(int id)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveProduct(Product product)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateProduct(Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}
