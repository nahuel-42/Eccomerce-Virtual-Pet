using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Backend.Modules.Connection.Domain.Services;
using Backend.Modules.Connection.Infrastructure.RabbitMQ;

namespace Backend.Modules.Connection.Infrastructure.Publishers
{
    public class RabbitMQPublisher : IMessagePublisher, IDisposable
    {
        private readonly IRabbitMQConnection _connection;
        private readonly RabbitMQSettings _settings;
        private IChannel? _channel;

        public RabbitMQPublisher(IRabbitMQConnection connection, RabbitMQSettings settings)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task PublishAsync<T>(T message, string routingKey) where T : class
        {
            await PublishAsync(message, _settings.ExchangeName, routingKey);
        }

        public async Task PublishAsync<T>(T message, string exchange, string routingKey) where T : class
        {
            const int maxRetries = 200;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    if (_channel == null)
                    {
                        Console.WriteLine("📡 Creando canal y configurando exchange...");
                        _channel = await _connection.CreateChannelAsync();

                        await _channel.ExchangeDeclareAsync(
                            exchange: exchange,
                            type: ExchangeType.Direct,
                            durable: true,
                            autoDelete: false,
                            arguments: null);
                    }

                    Console.WriteLine($"📨 Declarando y bindeando cola '{routingKey}' al exchange '{exchange}'...");

                    await _channel.QueueDeclareAsync(
                        queue: routingKey,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    await _channel.QueueBindAsync(
                        queue: routingKey,
                        exchange: exchange,
                        routingKey: routingKey,
                        arguments: null);

                    var properties = new BasicProperties
                    {
                        DeliveryMode = DeliveryModes.Persistent,
                        ContentType = "application/json",
                        Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    };

                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                    Console.WriteLine($"🚀 Intento #{attempt} - Publicando mensaje a routingKey '{routingKey}'...");

                    await _channel.BasicPublishAsync(
                        exchange: exchange,
                        routingKey: routingKey,
                        mandatory: false,
                        basicProperties: properties,
                        body: body);

                    Console.WriteLine("✅ Mensaje publicado exitosamente.");
                    return; // Publicación exitosa, salimos del método
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error en intento #{attempt}: {ex.GetType().Name} - {ex.Message}");

                    if (attempt == maxRetries)
                    {
                        Console.WriteLine("💥 Se alcanzó el número máximo de reintentos. Abortando publicación.");
                        throw;
                    }

                    // Espera exponencial: 1s, 2s, 4s
                    int delaySeconds = 5;
                    Console.WriteLine($"⏳ Esperando {delaySeconds} segundos antes de reintentar...");
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }
        }

        public async Task CloseAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
                _channel = null;
            }
        }

        public void Dispose()
        {
            //_channel?.Dispose();
        }
    }
}