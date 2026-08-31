using Microsoft.EntityFrameworkCore;
using ShopFlow.Enitities;

namespace ShopFlow.Data
{
    public class ShopFlowDbContext : DbContext
    {
        public ShopFlowDbContext(DbContextOptions<ShopFlowDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.UnitPrice)
                    .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.Id);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.Id);

                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.Items)
                    .HasForeignKey(ci => ci.CartId);

                entity.HasOne(ci => ci.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(ci => ci.ProductId);

                entity.Property(ci => ci.UnitPrice)
                    .HasPrecision(18, 2);
            });
        }
    }
}