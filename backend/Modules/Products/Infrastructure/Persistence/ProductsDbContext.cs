using Microsoft.EntityFrameworkCore;
using Backend.Modules.Products.Domain.Entities;
namespace Backend.Modules.Products.Infrastructure.Persistence{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<AnimalCategory> AnimalCategories { get; set; }
        public DbSet<ProductAnimalCategory> ProductAnimalCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("products");
            // Claves primarias
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();  // Marca el Id como autoincremental

            modelBuilder.Entity<AnimalCategory>().HasKey(ac => ac.Id);
            modelBuilder.Entity<AnimalCategory>()
                .Property(ac => ac.Id)
                .ValueGeneratedOnAdd();  // Marca el Id como autoincremental

            modelBuilder.Entity<ProductAnimalCategory>().HasKey(pac => pac.Id);
            modelBuilder.Entity<ProductAnimalCategory>()
                .Property(pac => pac.Id)
                .ValueGeneratedOnAdd();  // Marca el Id como autoincremental

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