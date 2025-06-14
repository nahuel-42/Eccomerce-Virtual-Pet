using Backend.Modules.Connection.Domain.Services;
using Backend.Modules.Connection.MessageContracts;
using Backend.Modules.Orders.Application.Interfaces;
using System.Text.Json;

namespace Backend.Modules.Connection.Infrastructure.Services
{
    public class MessageConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MessageConsumerService> _logger;

        public MessageConsumerService(
            IServiceProvider serviceProvider,
            ILogger<MessageConsumerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MessageConsumerService started");

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var consumer = scope.ServiceProvider.GetRequiredService<IMessageConsumer>();

                await consumer.StartConsumingAsync<UpdateOrderStatusContract>("update_status", async (orderRequest) =>
                {
                    _logger.LogInformation("Full message received on update_status: {OrderRequest}", JsonSerializer.Serialize(orderRequest));
                        using var handlerScope = _serviceProvider.CreateScope();
                        var orderService = handlerScope.ServiceProvider.GetRequiredService<IOrderCommands>();

                        _logger.LogInformation("Processing update_status request for Order Number: {OrderNumber}", orderRequest.orderNumber);

                        try
                        {
                            var createdOrder = await orderService.UpdateOrderStatusAsync(orderRequest);
                            _logger.LogInformation("Successfully processed update_status request. Updated Order ID: {OrderId}", createdOrder);

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing update_status request for Order Number: {OrderNumber}", orderRequest.orderNumber);
                            throw; // Re-throw para que RabbitMQ reencole el mensaje
                        }
                });

                // Mantener el servicio corriendo
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MessageConsumerService");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MessageConsumerService is stopping");
            
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var consumer = scope.ServiceProvider.GetRequiredService<IMessageConsumer>();
                await consumer.StopConsumingAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping MessageConsumerService");
            }

            await base.StopAsync(cancellationToken);
        }
    }
}