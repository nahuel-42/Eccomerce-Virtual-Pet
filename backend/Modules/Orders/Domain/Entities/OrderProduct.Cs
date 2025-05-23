using System.ComponentModel.DataAnnotations;
namespace Backend.Modules.Orders.Domain.Entities{
    public class OrderProduct
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int ProductQuantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public Order Order { get; set; } = null!;
    }
}