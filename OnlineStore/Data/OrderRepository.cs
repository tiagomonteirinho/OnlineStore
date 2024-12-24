using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Data.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models;

namespace OnlineStore.Data
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository // 'GenericRepository' cannot be instantiated (like an abstract class), so also inherit 'IOrderRepository' to be instantiated at Startup.cs where dependency injection requires an interface before class.
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<Order>> GetOrdersAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null) // If user doesn't exist
            {
                return null; // Return empty list.
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin")) // If user is admin
            {
                return _context.Orders // Get all orders.
                    .Include(o => o.Items) // Include all order items (order detail) // 'Include': join directly linked table.
                    .ThenInclude(i => i.Product) // Include all item products (order detail product) // 'ThenInclude': join table linked by intermediary table.
                    .Include(o => o.User) // Include order user object for name display.
                    .OrderByDescending(o => o.OrderDate);
            }

            return _context.Orders // Get all orders of logged in user.
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);

        }

        public async Task<IQueryable<OrderDetailTemp>> GetOrderDetailsTempAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return null;
            }

            return _context.OrderDetailsTemp
                .Include(od => od.Product)
                .Where(od => od.User == user)
                .OrderBy(od => od.Product.Name);
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null) // If user doesn't exist
            {
                return;
            }

            var product = await _context.Products.FindAsync(model.ProductId);
            if (product == null) // If product doesn't exist
            {
                return;
            }

            var orderDetailTemp = await _context.OrderDetailsTemp // Check if item already exists in order
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();

            if (orderDetailTemp == null) // If item doesn't exist
            {
                orderDetailTemp = new OrderDetailTemp // Create new item.
                {
                    User = user,
                    Product = product,
                    Price = product.Price,
                    Quantity = model.Quantity,
                };

                _context.OrderDetailsTemp.Add(orderDetailTemp); // Add item to order.
            }
            else // If item exists
            {
                orderDetailTemp.Quantity += model.Quantity; // Add new quantity to item line.
                _context.OrderDetailsTemp.Update(orderDetailTemp); // Update item.
            }

            await _context.SaveChangesAsync(); // Save changes to database.
        }

        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity) // Modify item quantity.
        {
            var orderDetailTemp = await _context.OrderDetailsTemp.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0) // If quantity is higher than 0
            {
                _context.OrderDetailsTemp.Update(orderDetailTemp);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteOrderDetailTempAsync(int id) // Delete item.
        {
            var orderDetailTemp = await _context.OrderDetailsTemp.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            _context.OrderDetailsTemp.Remove(orderDetailTemp); // Remove item from order.
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return false;
            }

            var orderDetailsTemp = await _context.OrderDetailsTemp // Temporary list of order details.
                .Include(od => od.Product)
                .Where(od => od.User == user)
                .ToListAsync();

            if(orderDetailsTemp == null || orderDetailsTemp.Count == 0) // If list is null or empty
            {
                return false;
            }

            var orderDetails = orderDetailsTemp.Select(od => new OrderDetail // For each order detail in temporary list
            {
                Price = od.Price,
                Product = od.Product,
                Quantity = od.Quantity,
            }).ToList(); // Convert to OrderDetail and add to definitive list.

            var order = new Order
            {
                OrderDate = DateTime.UtcNow, // Standard date/time format.
                User = user,
                Items = orderDetails
            };

            await CreateAsync(order); // Create order
            _context.OrderDetailsTemp.RemoveRange(orderDetailsTemp); // Remove temporary order details list to clear view form.
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeliverOrder(DeliveryViewModel model)
        {
            var order = await _context.Orders.FindAsync(model.Id);
            if (order == null)
            {
                return;
            }

            order.DeliveryDate = model.DeliveryDate;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
    }
}
