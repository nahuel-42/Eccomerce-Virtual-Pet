using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Orders.Application.Interfaces;
using Backend.Modules.Orders.Infrastructure.Persistence;
using Backend.Modules.Orders.Application.Factories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules.Orders.Application.Queries {

    public class OrderCommands : IOrderCommands
    {
        private readonly OrdersDbContext _context;
        private readonly OrderFactory _orderFactory;
        private readonly OrderUpdater _orderUpdater;

        public OrderCommands(OrdersDbContext context, OrderFactory orderFactory, OrderUpdater orderUpdater)
        {
            _context = context;
            _orderFactory = orderFactory;
            _orderUpdater = orderUpdater;
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = await _orderFactory.Create(createOrderDto);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        public async Task UpdateOrderStatusAsync(int orderId, UpdateOrderDto updateOrderDto)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            _orderUpdater.Update(order, updateOrderDto);

            await _context.SaveChangesAsync();
        }
    }
}