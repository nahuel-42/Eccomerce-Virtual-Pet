using Microsoft.EntityFrameworkCore;
using Backend.Modules.Orders.Domain.Entities;

namespace Backend.Modules.Orders.Infrastructure.Persistence{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("orders");
            // Claves primarias
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();  // Marca el Id como autoincremental

            modelBuilder.Entity<OrderProduct>().HasKey(op => op.Id);
            modelBuilder.Entity<OrderProduct>()
                .Property(op => op.Id)
                .ValueGeneratedOnAdd();  // Marca el Id como autoincremental

            modelBuilder.Entity<OrderStatus>().HasKey(os => os.Id);
            modelBuilder.Entity<OrderStatus>()
                .Property(os => os.Id)
                .ValueGeneratedOnAdd();  // Marca el Id como autoincremental

            // Relaciones
            modelBuilder.Entity<Order>()
                .HasOne(o => o.OrderStatus)
                .WithMany(os => os.Orders)
                .HasForeignKey(o => o.OrderStatusId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            base.OnModelCreating(modelBuilder);
        }
    }

}