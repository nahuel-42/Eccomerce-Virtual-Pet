// Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
using Backend.Models.DTOS;
using Backend.Modules.Products.Infrastructure.Persistence;

namespace ProductsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductQueries _productQueries;

        public ProductsController(IProductQueries productQueries)
        {
            _productQueries = productQueries;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            var products = await _productQueries.GetAllAsync();

            if (products == null || products.Count == 0)
                return NotFound("No products found.");

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _productQueries.GetByIdAsync(id);

            if (product == null)
                return NotFound($"Product with ID {id} not found.");

            return Ok(product);
        }
    }
}
