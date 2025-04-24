
using System.ComponentModel.DataAnnotations;
public class OrderStatus
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
