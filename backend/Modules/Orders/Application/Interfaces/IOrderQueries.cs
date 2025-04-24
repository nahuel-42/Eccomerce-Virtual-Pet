// En algún archivo como Application/Contracts/IOrderQueries.cs
using Backend.Models.DTOS;
using Backend.Modules.Orders.Infrastructure.Persistence;

public interface IOrderQueries
{
    Task<List<OrderDto>> GetOrdersAsync();
}
