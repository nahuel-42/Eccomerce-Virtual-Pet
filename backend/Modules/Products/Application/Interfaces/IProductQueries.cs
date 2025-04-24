// Modules/Product/Application/Contracts/IProductQueries.cs
using Backend.Models.DTOS;

public interface IProductQueries
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
}
