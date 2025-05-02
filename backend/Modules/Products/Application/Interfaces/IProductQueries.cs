// Modules/Product/Application/Contracts/IProductQueries.cs
using Backend.Shared.DTOs;

namespace Backend.Modules.Products.Application.Interfaces {
    public interface IProductQueries
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<List<ProductDto>> GetMultipleByIdAsync(List<int> ids);
    }
}

