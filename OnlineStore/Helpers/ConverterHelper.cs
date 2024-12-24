using System.IO;
using OnlineStore.Data.Entities;
using OnlineStore.Models;

namespace OnlineStore.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Product ConvertToProduct(ProductViewModel model, string path, bool isNew)
        {
            return new Product
            {
                Id = isNew ? 0 : model.Id, // If model is new (for create), set model ID as 0, otherwise (for edit) get model ID.
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
