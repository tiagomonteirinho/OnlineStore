using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using OnlineStore.Data.Entities;

namespace OnlineStore.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    { // Specific class methods not existing at IGenericRepository.

        public IQueryable GetAllWithUser(); // Get data from different table.

        IEnumerable<SelectListItem> GetComboBoxProducts();
    }
}
