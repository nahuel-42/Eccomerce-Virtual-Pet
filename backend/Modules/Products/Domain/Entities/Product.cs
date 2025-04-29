
using System.ComponentModel.DataAnnotations;
namespace Backend.Modules.Products.Domain.Entities{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public string? Description { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; } 
        public ICollection<ProductAnimalCategory> ProductAnimalCategories { get; set; } = new List<ProductAnimalCategory>();
    }
}