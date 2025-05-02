using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Orders.Application.Interfaces;
using Backend.Modules.Orders.Infrastructure.Persistence;
using Backend.Modules.Orders.Application.Factories;
using Backend.Modules.Products.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules.Orders.Application.Queries {

    public class OrderCommands : IOrderCommands
    {
        private readonly OrdersDbContext _context;
        private readonly OrderFactory _orderFactory;
        private readonly OrderUpdater _orderUpdater;
        private readonly IProductCommands _productCommands;

        public OrderCommands(OrdersDbContext context, OrderFactory orderFactory, OrderUpdater orderUpdater, IProductCommands productCommands)
        {
            _context = context;
            _orderFactory = orderFactory;
            _orderUpdater = orderUpdater;
            _productCommands = productCommands;
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _orderFactory.Create(createOrderDto);

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Descontar stock para cada producto
                foreach (var orderProduct in order.OrderProducts)
                {
                    await _productCommands.DecreaseStockAsync(orderProduct.ProductId, orderProduct.ProductQuantity);
                }

                // Confirmar la transacción
                await transaction.CommitAsync();
                return order.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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