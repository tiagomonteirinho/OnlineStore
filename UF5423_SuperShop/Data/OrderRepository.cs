using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UF5423_SuperShop.Data.Entities;
using UF5423_SuperShop.Helpers;
using UF5423_SuperShop.Models;

namespace UF5423_SuperShop.Data
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository // 'IOrderRepository' for instantiation at Startup.cs where dependency injection requires interface before class.
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
                    .Include(o => o.Items) // Join all order items (order detail) // 'Include': join directly linked table.
                    .ThenInclude(i => i.Product) // Join all item products (order detail product) // 'ThenInclude': join table linked by intermediary table.
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

                _context.OrderDetailsTemp.Add(orderDetailTemp); // Create new item line.
            }
            else // If item exists
            {
                orderDetailTemp.Quantity += model.Quantity; // Add new quantity to item line.
                _context.OrderDetailsTemp.Update(orderDetailTemp); // Update item line.
            }

            await _context.SaveChangesAsync(); // Save to database.
        }

        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity) // Modify item quantity.
        {
            var orderDetailTemp = await _context.OrderDetailsTemp.FindAsync(id);
            if (orderDetailTemp == null) // If item doesn't exist
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
    }
}
