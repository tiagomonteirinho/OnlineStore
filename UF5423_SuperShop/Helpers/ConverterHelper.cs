using System.IO;
using UF5423_SuperShop.Data.Entities;
using UF5423_SuperShop.Models;

namespace UF5423_SuperShop.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Product ConvertToProductModel(ProductViewModel model, string path, bool isNew)
        {
            return new Product
            {
                Id = isNew ? 0 : model.Id, // If product is new (for create), set product ID as 0, otherwise (for edit) get product ID.
                Name = model.Name,
                Price = model.Price,
                ImageUrl = path,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                IsAvailable = model.IsAvailable,
                Stock = model.Stock,
                User = model.User
            };
        }

        public ProductViewModel ConvertToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                IsAvailable = product.IsAvailable,
                Stock = product.Stock,
                User = product.User
            };
        }
    }
}
