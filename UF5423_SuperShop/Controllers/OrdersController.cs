using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UF5423_SuperShop.Data;
using UF5423_SuperShop.Models;

namespace UF5423_SuperShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _orderRepository.GetOrdersAsync(this.User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create() // Temporary create view that leads to original create (post) view.
        {
            var model = await _orderRepository.GetOrderDetailsTempAsync(this.User.Identity.Name);
            return View(model);
        }

        public IActionResult AddProduct()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Products = _productRepository.GetComboBoxProducts()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
            }

            return View(model);
        }

        public async Task<IActionResult> IncreaseItemQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound(); //TODO: Replace by ItemNotFound view.
            }

            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, 1); // Get item quantity + 1.
            return RedirectToAction("Create"); // Refresh/update view.
        }

        public async Task<IActionResult> DecreaseItemQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound(); //TODO: Replace by ItemNotFound view.
            }

            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1); // Get item quantity + (-1).
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound(); //TODO: Replace by ItemNotFound view.
            }

            await _orderRepository.DeleteOrderDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmOrder()
        {
            var response = await _orderRepository.ConfirmOrderAsync(this.User.Identity.Name);
            if (response)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }
    }
}
