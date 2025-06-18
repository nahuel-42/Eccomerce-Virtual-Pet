using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Orders.Application.Interfaces;
using Backend.Modules.Orders.Infrastructure.Persistence;
using Backend.Modules.Orders.Application.Factories;
using Backend.Modules.Products.Application.Interfaces;
using Backend.Modules.Orders.Application.Events;
using Backend.Modules.Orders.Domain.Enums;
using Backend.Modules.Connection.MessageContracts;
using Backend.Modules.Orders.Domain.Entities;
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
            var order = await ValidateMessage(updateOrderStatusContract);
            var orderStatus = OrderStatusAdapter(updateOrderStatusContract);

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
            return order.Id;
        }
        private OrderStatusEnum OrderStatusAdapter(UpdateOrderStatusContract updateOrderStatusContract)
        {
            return updateOrderStatusContract.Status switch
            {
                1 => OrderStatusEnum.Recibido,
                2 => OrderStatusEnum.Procesando,
                3 => OrderStatusEnum.EnCamino,
                4 => OrderStatusEnum.Entregado,
                5 => OrderStatusEnum.FallaEntrega,
                _ => throw new ArgumentOutOfRangeException(nameof(updateOrderStatusContract.Status),
                    $"Unknown status from broker: {updateOrderStatusContract.Status}")
            };
        }
        
        private async Task<Order> ValidateMessage(UpdateOrderStatusContract contract)
        {
            var errores = new List<string>();

            // Validación 1: orderNumber
            if (string.IsNullOrWhiteSpace(contract.OrderNumber))
            {
                errores.Add("El campo 'OrderNumber' no puede ser nulo o vacío.");
            }

            // Validación 2: parseo de ID
            if (!int.TryParse(contract.OrderNumber, out int orderId))
            {
                errores.Add($"El campo 'orderNumber' debe ser un número válido. Valor recibido: {contract.OrderNumber}");
                // No tiene sentido seguir validando si no tenemos ID
                if (errores.Any())
                    throw new ArgumentException(string.Join(" | ", errores));
                return null!;
            }

            // Validación 3: existencia en DB
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                errores.Add($"No se encontró la orden con ID {orderId}.");
            }

            // Validación 4: transición de estado
            OrderStatusEnum newStatus;
            try
            {
                newStatus = OrderStatusAdapter(contract);
            }
            catch (ArgumentOutOfRangeException)
            {
                errores.Add($"El estado recibido '{contract.Status}' no es válido.");
                // No tiene sentido seguir sin estado válido
                throw new ArgumentException(string.Join(" | ", errores));
            }

            if (order != null)
            {
                var currentStatus = order.OrderStatusId;

                var transicionesValidas = new Dictionary<int, List<int>>
                {
                    { (int)OrderStatusEnum.Recibido,       new() { (int)OrderStatusEnum.Procesando } },
                    { (int)OrderStatusEnum.Procesando,     new() { (int)OrderStatusEnum.EnCamino } },
                    { (int)OrderStatusEnum.EnCamino,       new() { (int)OrderStatusEnum.Entregado, (int)OrderStatusEnum.FallaEntrega } },
                    { (int)OrderStatusEnum.FallaEntrega,   new() { (int)OrderStatusEnum.EnCamino } }
                };

                if (!transicionesValidas.TryGetValue(currentStatus, out var posibles) || !posibles.Contains((int)newStatus))
                {
                    errores.Add($"No se puede cambiar de estado desde '{(OrderStatusEnum)currentStatus}' a '{newStatus}'.");
                }
            }

            if (errores.Any())
            {
                throw new ArgumentException(string.Join(" | ", errores));
            }

            return order!;
        }

    }
}