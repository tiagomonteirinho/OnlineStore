using OnlineStore.Data.Entities;
using OnlineStore.Models;

namespace OnlineStore.Helpers
{
    public interface IConverterHelper
    {
        Product ConvertToProduct(ProductViewModel model, string path, bool isNew);

        ProductViewModel ConvertToProductViewModel(Product product);
    }
}
