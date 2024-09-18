using System.Linq;
using System.Threading.Tasks;
using UF5423_SuperShop.Data.Entities;
using UF5423_SuperShop.Models;

namespace UF5423_SuperShop.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrdersAsync(string username); //Get list of all orders.

        Task<IQueryable<OrderDetailTemp>> GetOrderDetailsTempAsync(string username);

        Task AddItemToOrderAsync(AddItemViewModel model, string username);

        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity); // Sum product quantities if same product is added multiple times.

        Task DeleteOrderDetailTempAsync(int id);
    
    }
}
