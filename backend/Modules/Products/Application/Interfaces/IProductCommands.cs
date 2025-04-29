using Backend.Modules.Products.Application.DTOs;

namespace Backend.Modules.Products.Application.Interfaces {
    
    public interface IProductCommands
    {
        Task DecreaseStockAsync(int productId, int quantity);
    }
}

