namespace Backend.Modules.Orders.Application.DTOs {

    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public int Status { get; set; }
        public string Phone { get; set; } = null!;
        public decimal? TotalPrice { get; set; }
        public string Address { get; set; } = null!;
        public OrderUserDto User { get; set; } = null!;
        public List<OrderProductDto> Products { get; set; } = new();
    }

    public class OrderProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}