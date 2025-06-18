using RabbitMQ.Client;

namespace Backend.Modules.Connection.Infrastructure.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        Task<bool> IsConnectedAsync();
        Task<IChannel> CreateChannelAsync();
        Task CloseAsync();
    }
}