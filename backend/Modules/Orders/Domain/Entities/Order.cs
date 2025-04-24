using System.ComponentModel.DataAnnotations;

public class Order
{
    public int Id { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    public DateTime? DeliveredDate { get; set; }

    [Required]
    public int OrderStatusId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public string Address { get; set; } = null!;

    [Required]
    public string Phone { get; set; } = null!;
    public OrderStatus OrderStatus { get; set; } = null!;
    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
