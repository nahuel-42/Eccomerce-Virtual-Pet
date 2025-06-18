using Backend.Modules.Connection.Domain.Services;
using Backend.Modules.Connection.MessageContracts;
using Backend.Modules.Orders.Application.Interfaces;
using Backend.Modules.Connection.Domain.Services;
using System.Text.Json;

namespace Backend.Modules.Connection.Infrastructure.Services
{
    public class MessageConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MessageConsumerService> _logger;

        public MessageConsumerService(
            IServiceProvider serviceProvider,
            ILogger<MessageConsumerService> logger) // ✅ nuevo
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

                    try
                    {
                        using var handlerScope = _serviceProvider.CreateScope();
                        var orderService = handlerScope.ServiceProvider.GetRequiredService<IOrderCommands>();

                        _logger.LogInformation("Processing update_status request for Order Number: {OrderNumber}", orderRequest.orderNumber);

                        var createdOrder = await orderService.UpdateOrderStatusAsync(orderRequest);

                        _logger.LogInformation("Successfully processed update_status request. Updated Order ID: {OrderId}", createdOrder);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing update_status request for Order Number: {OrderNumber}", orderRequest.orderNumber);
                        using var errorScope = _serviceProvider.CreateScope();
                        var publisher = errorScope.ServiceProvider.GetRequiredService<IMessagePublisher>();
                        // ✅ publicar mensaje de error
                        var errorMessage = new
                        {
                            OriginalMessage = orderRequest,
                            Error = ex.Message,
                            Timestamp = DateTime.UtcNow
                        };
                        await publisher.PublishAsync(errorMessage, "update_status_errors");

                        throw; // ❗ rethrow para reencolar si lo necesitás
                    }
                });

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
