namespace Backend.Modules.Connection.Infrastructure.RabbitMQ
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; } = "host.docker.internal";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string VirtualHost { get; set; } = "/";
        public string ExchangeName { get; set; } = "orders.exchange";
    }
}