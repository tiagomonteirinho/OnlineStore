﻿using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Data.Entities;
using OnlineStore.Models;

namespace OnlineStore.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrdersAsync(string username); //Get list of all orders.

        Task<IQueryable<OrderDetailTemp>> GetOrderDetailsTempAsync(string username);

        Task AddItemToOrderAsync(AddItemViewModel model, string username);

        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity); // Sum product quantities if same product is added multiple times.

        Task DeleteOrderDetailTempAsync(int id);
    
        Task<bool> ConfirmOrderAsync(string username);

        Task DeliverOrder(DeliveryViewModel model);

        Task<Order> GetOrderAsync(int id); // Get specific order.
    }
}
