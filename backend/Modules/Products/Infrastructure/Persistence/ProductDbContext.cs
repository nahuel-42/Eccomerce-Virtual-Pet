using Microsoft.EntityFrameworkCore;
using Backend.Modules.Products.Domain.Entities;
namespace Backend.Modules.Products.Infrastructure.Persistence{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<AnimalCategory> AnimalCategories { get; set; }
        public DbSet<ProductAnimalCategory> ProductAnimalCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Claves primarias
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<AnimalCategory>().HasKey(ac => ac.Id);
            modelBuilder.Entity<ProductAnimalCategory>().HasKey(pac => pac.Id);

            // Relaciones
            modelBuilder.Entity<ProductAnimalCategory>()
                .HasOne(pac => pac.Product)
                .WithMany(p => p.ProductAnimalCategories)
                .HasForeignKey(pac => pac.ProductId);

            modelBuilder.Entity<ProductAnimalCategory>()
                .HasOne(pac => pac.AnimalCategory)
                .WithMany(ac => ac.ProductAnimalCategories)
                .HasForeignKey(pac => pac.AnimalCategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }

}