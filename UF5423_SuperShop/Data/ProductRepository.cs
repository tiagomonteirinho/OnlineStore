using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public IEnumerable<SelectListItem> GetComboBoxProducts() //'SelectListItem': HTML '<option>' or '<select>'.
        {
            var list = _context.Products.Select(p => new SelectListItem // For each product in '_context'
            {
                Text = p.Name, // 'option' text.
                Value = p.Id.ToString(),
            }).ToList(); // Add product to list as <option>.

            list.Insert(0, new SelectListItem // First item with ID 0, whose addition is not allowed at AddItemViewModel.cs.
            {
                Text = "(Select a product...)",
                Value = "0"
            });

            return list;
        }
    }
}
