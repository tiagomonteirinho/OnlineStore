using Microsoft.AspNetCore.Authentication.JwtBearer;  // 'JwtBearer': token type.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data;

namespace OnlineStore.Controllers.API
{
    [Route("api/[controller]")] // Example: 'https://localhost:44333/api/products'.
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Only authorize users with token.
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
            return Ok(_productRepository.GetAllWithUser()); // Return products from 'IProductRepository' converted to JSON.
        }
    }
}
