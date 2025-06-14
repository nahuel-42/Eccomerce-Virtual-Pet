using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Orders.Application.Interfaces;
using Backend.Modules.Orders.Infrastructure.Persistence;
using Backend.Modules.Orders.Application.Factories;
using Backend.Modules.Products.Application.Interfaces;
using Backend.Modules.Orders.Application.Events;
using Backend.Modules.Orders.Domain.Enums;
using Backend.Modules.Connection.MessageContracts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules.Orders.Application.Queries {

    public class OrderCommands : IOrderCommands
    {
        private readonly OrdersDbContext _context;
        private readonly OrderFactory _orderFactory;
        private readonly OrderUpdater _orderUpdater;
        private readonly IProductCommands _productCommands;
        private readonly IOrderEventPublisher _eventPublisher;
        private readonly ILogger<OrderCommands> _logger;

        public OrderCommands(OrdersDbContext context, OrderFactory orderFactory, OrderUpdater orderUpdater, IProductCommands productCommands, IOrderEventPublisher eventPublisher, ILogger<OrderCommands> logger)
        {
            _context = context;
            _orderFactory = orderFactory;
            _orderUpdater = orderUpdater;
            _productCommands = productCommands;
            _eventPublisher = eventPublisher;
            _logger = logger;
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
                await _eventPublisher.PublishOrderCreatedAsync(order);
                return order.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> UpdateOrderStatusAsync(UpdateOrderStatusContract updateOrderStatusContract)
        {   
            var orderId = int.Parse(updateOrderStatusContract.orderNumber);
            var order = await _context.Orders.FindAsync(orderId);
            var orderStatus = OrderStatusAdapter(updateOrderStatusContract);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            _orderUpdater.UpdateStatus(order, orderStatus);
                // 🔍 LOG DE CAMBIOS DETECTADOS POR EF CORE
                var entry = _context.Entry(order);
                _logger.LogInformation("Order state: {State}", entry.State);
                foreach (var prop in entry.Properties)
                {
                    _logger.LogInformation("Property {Property}: Current={Current}, Original={Original}, Modified={Modified}",
                        prop.Metadata.Name,
                        prop.CurrentValue,
                        prop.OriginalValue,
                        prop.IsModified);
                }

            await _context.SaveChangesAsync();
            return orderId;
        }
        private OrderStatusEnum OrderStatusAdapter(UpdateOrderStatusContract updateOrderStatusContract)
        {
            return updateOrderStatusContract.status switch
            {
                "RECEIVED"         => OrderStatusEnum.Recibido,
                "READY_TO_SHIP"    => OrderStatusEnum.Procesando,
                "OUT_FOR_DELIVERY" => OrderStatusEnum.EnCamino,
                "DELIVERED"        => OrderStatusEnum.Entregado,
                "DELIVERY_FAILED"  => OrderStatusEnum.FallaEntrega,
                _ => throw new ArgumentOutOfRangeException(nameof(updateOrderStatusContract.status),
                    $"Unknown status from broker: {updateOrderStatusContract.status}")
            };
        }
    }
}