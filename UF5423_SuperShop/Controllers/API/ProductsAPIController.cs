using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UF5423_SuperShop.Data;

namespace UF5423_SuperShop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsAPIController : Controller // Products API Controller.
    {
        private readonly IProductRepository _productRepository;

        public ProductsAPIController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_productRepository.GetAllWithUser()); // Return products from 'IProductRepository' converted to JSON.
        }
    }
}
