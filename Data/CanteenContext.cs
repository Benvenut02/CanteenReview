using CanteenReview.Models;
using Microsoft.EntityFrameworkCore;

namespace CanteenReview.Data
{
    public class CanteenContext : DbContext
    {
        public CanteenContext(DbContextOptions<CanteenContext> options) : base(options) { }

        public DbSet<FoodItem> FootItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
