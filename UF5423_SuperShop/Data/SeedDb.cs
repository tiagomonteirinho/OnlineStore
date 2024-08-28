using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); // Create database if there is none.

            if (!_context.Products.Any()) // If data base is empty
            {
                AddProduct("Galaxy A53");
                AddProduct("Galaxy Tab A6");
                AddProduct("Galaxy Watch 7");
                AddProduct("Galaxy Buds 3");
                AddProduct("Galaxy Book 4");
                await _context.SaveChangesAsync(); // Save default data to data base.
            }
        }

        private void AddProduct(string productName) // Define default data.
        {
            _context.Products.Add(new Product
            {
                ProductName = productName,
                ProductPrice = _random.Next(99, 1499),
                ProductIsAvailable = true,
                ProductStock = _random.Next(99, 999),
            });
        }
    }
}