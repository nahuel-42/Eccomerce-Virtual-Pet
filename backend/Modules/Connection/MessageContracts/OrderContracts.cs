using Backend.Modules.Orders.Domain.Enums;
namespace Backend.Modules.Connection.MessageContracts
{
    // Evento SALIENTE: Envio la orden creada al broker
    public class OrderCreatedContract
    {
        public string OrderNumber { get; set; } 
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<OrderItemContract> Items { get; set; } = new();
    }
    public class OrderItemContract
    {
        public string ProductId { get; set; } 
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    // Recibo update de estado de orden
    public class UpdateOrderStatusContract
    {
        public string orderNumber { get; set; }

        public string status { get; set; }
    }


}