using Microsoft.EntityFrameworkCore;
using Backend.Modules.Cart.Infrastructure.Persistence.Entities;

namespace Backend.Modules.Cart.Infrastructure.Persistence
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) { }

        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO
            base.OnModelCreating(modelBuilder);
        }
    }
}
