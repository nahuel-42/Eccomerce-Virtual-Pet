// Modules/Product/Application/Services/ProductQueries.cs
using Backend.Models.DTOS;
using Microsoft.EntityFrameworkCore;
using Backend.Modules.Products.Infrastructure.Persistence;

public class ProductQueries : IProductQueries
{
    private readonly ProductDbContext _context;

    public ProductQueries(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        return await _context.Products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Cost = p.Price
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Cost = p.Price
            })
            .FirstOrDefaultAsync();
    }
}
