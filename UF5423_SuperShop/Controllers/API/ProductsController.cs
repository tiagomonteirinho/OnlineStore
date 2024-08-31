using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UF5423_SuperShop.Data;

namespace UF5423_SuperShop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller // Products API Controller.
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_productRepository.GetAll()); // Return products from 'IProductRepository' converted to JSON.
        }
    }
}
