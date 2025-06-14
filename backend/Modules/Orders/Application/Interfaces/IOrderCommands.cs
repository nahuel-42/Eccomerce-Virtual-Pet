using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Connection.MessageContracts;

namespace Backend.Modules.Orders.Application.Interfaces {
    public interface IOrderCommands
    {
        Task<int> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<int> UpdateOrderStatusAsync(UpdateOrderStatusContract updateOrderStatusContract);
    }
}