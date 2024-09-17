using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public IQueryable GetAllWithUser(); // Get data from different table.

        IEnumerable<SelectListItem> GetComboBoxProducts();
    }
}
