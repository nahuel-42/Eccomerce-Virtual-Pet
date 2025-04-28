using Backend.Modules.Orders.Application.DTOs;

namespace Backend.Modules.Orders.Application.Interfaces {
    public interface IOrderCommands
    {
        Task<int> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task UpdateOrderStatusAsync(int orderId, UpdateOrderDto updateOrderDto);
        Task DeleteOrderAsync(int orderId);
    }
}