// Modules/Product/Application/Services/ProductQueries.cs
using Backend.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Backend.Modules.Products.Infrastructure.Persistence;
using Backend.Modules.Products.Application.Interfaces;

namespace Backend.Modules.Products.Application.Queries {
        
    public class ProductQueries : IProductQueries
    {
        private readonly ProductsDbContext _context;

        public ProductQueries(ProductsDbContext context)
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
                    ImageUrl = p.ImageUrl,
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
                    ImageUrl = p.ImageUrl,
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

        public async Task<List<ProductDto>> GetMultipleByIdAsync(List<int> ids)
        {
            return await _context.Products
                .Include(p => p.ProductAnimalCategories)
                    .ThenInclude(pac => pac.AnimalCategory)
                .Where(p => ids.Contains(p.Id))
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
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
    }
}