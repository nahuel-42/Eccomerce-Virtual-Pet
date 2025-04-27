// Modules/Product/Application/Services/ProductQueries.cs
using Backend.Modules.Products.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using Backend.Modules.Products.Infrastructure.Persistence;
using Backend.Modules.Products.Application.Interfaces;

namespace Backend.Modules.Products.Application.Queries {
        
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
                .Include(p => p.ProductAnimalCategories)
                    .ThenInclude(pac => pac.AnimalCategory)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    Description = p.Description,
                    AnimalCategories = p.ProductAnimalCategories
                        .Select(pac => new AnimalCategoryDto
                        {
                            Id = pac.AnimalCategory.Id,
                            Name = pac.AnimalCategory.Name
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductAnimalCategories)
                    .ThenInclude(pac => pac.AnimalCategory)
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    Description = p.Description,
                    AnimalCategories = p.ProductAnimalCategories
                        .Select(pac => new AnimalCategoryDto
                        {
                            Id = pac.AnimalCategory.Id,
                            Name = pac.AnimalCategory.Name
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}