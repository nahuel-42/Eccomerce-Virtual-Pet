using System.ComponentModel.DataAnnotations;

namespace Backend.Modules.Products.Domain.Entities{
    public class AnimalCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<ProductAnimalCategory> ProductAnimalCategories { get; set; } = new List<ProductAnimalCategory>();
    }
}