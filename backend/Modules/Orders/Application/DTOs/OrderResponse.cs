namespace Backend.Modules.Orders.Application.DTOs {

    public class OrderResponse
    {
        public string Message { get; set; } = null!;
        public int? OrderId { get; set; } 
    }
}