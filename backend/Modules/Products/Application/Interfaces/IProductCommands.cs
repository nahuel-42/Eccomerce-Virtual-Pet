using Backend.Shared.DTOs;

namespace Backend.Modules.Products.Application.Interfaces {
    
    public interface IProductCommands
    {
        Task DecreaseStockAsync(int productId, int quantity);
    }
}

