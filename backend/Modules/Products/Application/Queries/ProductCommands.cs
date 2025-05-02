using Backend.Shared.DTOs;
using Backend.Modules.Products.Application.Interfaces;
using Backend.Modules.Products.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules.Products.Application.Queries {

    public class ProductCommands : IProductCommands
    {
        private readonly ProductsDbContext _context;

        public ProductCommands(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task DecreaseStockAsync(int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            if (product.Stock < quantity)
                throw new InvalidOperationException($"Insufficient stock for product ID {productId}. Available: {product.Stock}, Requested: {quantity}");

            product.Stock -= quantity;
            await _context.SaveChangesAsync();
        }
    }
}