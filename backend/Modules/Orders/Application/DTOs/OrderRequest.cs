namespace Backend.Modules.Orders.Application.DTOs {

    public class CreateOrderDto
    {
        public List<CreateOrderProductDto> Products { get; set; } = new();
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int UserId { get; set; }
    }

    public class CreateOrderProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateOrderDto
    {
        public string? Address { get; set; } = null!;
        public string? Phone { get; set; } = null!;
        public int? OrderStatusId { get; set; }
    }

}