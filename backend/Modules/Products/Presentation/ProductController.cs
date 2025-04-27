using Microsoft.AspNetCore.Mvc;
using Backend.Modules.Products.Application.Interfaces;

namespace Backend.Modules.Cart.Presentation
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductQueries _productQueries;

        public ProductController(IProductQueries productQueries)
        {
            _productQueries = productQueries;
        }

        // Obtener todos los productos
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productQueries.GetAllAsync();
            return Ok(products);
        }

        // Obtener un producto por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productQueries.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }
    }
}
