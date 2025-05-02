using System.ComponentModel.DataAnnotations;
namespace Backend.Modules.Products.Domain.Entities{
    public class ProductAnimalCategory
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int AnimalCategoryId { get; set; }

        public Product Product { get; set; } = null!;
        public AnimalCategory AnimalCategory { get; set; } = null!;
    }
}