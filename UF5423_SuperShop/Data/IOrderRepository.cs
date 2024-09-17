using System.Linq;
using System.Threading.Tasks;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrdersAsync(string username); //Get list of all orders.

        Task<IQueryable<OrderDetailTemp>> GetOrderDetailsTempAsync(string username);
    }
}
