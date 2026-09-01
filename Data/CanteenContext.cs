using CanteenReview.Models;
using Microsoft.EntityFrameworkCore;

namespace CanteenReview.Data
{
    public class CanteenContext : DbContext
    {
        public CanteenContext(DbContextOptions<CanteenContext> options) : base(options) { }

        public DbSet<FoodItem> FoodItems { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FoodItem>()
                .Property(f => f.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<FoodItem>()
                .HasMany(f => f.Reviews)
                .WithOne(r => r.FoodItem)
                .HasForeignKey(r => r.FoodItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
