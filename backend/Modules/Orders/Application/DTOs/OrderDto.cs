namespace Backend.Modules.Orders.Application.DTOs {

    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string Status { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public float? TotalPrice { get; set; }
        public List<OrderProductDetailDto> Products { get; set; } = new();
    }

    public class OrderProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}