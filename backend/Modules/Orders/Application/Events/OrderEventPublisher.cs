using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.Modules.Orders.Domain.Entities;
using Backend.Modules.Orders.Domain.Enums;
using Backend.Modules.Connection.Domain.Services;
using Backend.Modules.Connection.MessageContracts;
using Backend.Modules.Products.Application.Interfaces;
using Backend.Modules.Users.Application.Interfaces;
using RabbitMQ.Client;

namespace Backend.Modules.Orders.Application.Events
{
    public class OrderEventPublisher : IOrderEventPublisher
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly IProductQueries _productQueries;
        private readonly IUserQueries _userQueries;
        public OrderEventPublisher(IMessagePublisher messagePublisher, IProductQueries productQueries, IUserQueries userQueries)
        {
            _messagePublisher = messagePublisher;
            _productQueries = productQueries;
            _userQueries = userQueries;
        }

        public async Task PublishOrderCreatedAsync(Order order)
        {
            Console.WriteLine($"📦 [PublishOrderCreatedAsync] Iniciando publicación para OrderId={order.Id}");

            var contract = await MapToOrderCreatedContractAsync(order);

            Console.WriteLine($"✅ [PublishOrderCreatedAsync] Contrato creado: OrderNumber={contract.OrderNumber}, Items={contract.Items.Count}");

            _messagePublisher.PublishAsync(contract, "create_order");

            Console.WriteLine("🚀 [PublishOrderCreatedAsync] Mensaje publicado exitosamente a RabbitMQ con routingKey 'create_order'");
        }

        private async Task<OrderCreatedContract> MapToOrderCreatedContractAsync(Order order)
        {
            var items = new List<OrderItemContract>();
            var user = await _userQueries.GetUserByIdAsync(order.UserId);

            foreach (var item in order.OrderProducts)
            {
                var product = await _productQueries.GetByIdAsync(item.ProductId);

                items.Add(new OrderItemContract
                {
                    ProductId = item.ProductId.ToString(),
                    ProductName = product.Name,
                    Quantity = item.ProductQuantity

                });
            }

            return new OrderCreatedContract
            {
                OrderNumber = order.Id.ToString(),
                CustomerName = user.Name,
                CustomerEmail = user.Email,
                Address = order.Address,
                Phone = order.Phone,
                Items = items
            };
        }
    }

}