// En algún archivo como Application/Contracts/IOrderQueries.cs
using Backend.Modules.Orders.Application.DTOs;

namespace Backend.Modules.Orders.Application.Interfaces {
    public interface IOrderQueries
    {
        Task<List<OrderDto>> GetOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(Guid id);
    }
}
