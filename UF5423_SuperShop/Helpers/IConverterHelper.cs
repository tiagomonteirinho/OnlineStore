using UF5423_SuperShop.Data.Entities;
using UF5423_SuperShop.Models;

namespace UF5423_SuperShop.Helpers
{
    public interface IConverterHelper
    {
        Product ConvertToProduct(ProductViewModel model, string path, bool isNew);

        ProductViewModel ConvertToProductViewModel(Product product);
    }
}
