using System.ComponentModel.DataAnnotations;

namespace CanteenReview.Models
{
    public class FoodItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        // This links the FoodItem to its reviews
        public List<Review> Reviews { get; set; }
    }

    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentName { get; set; } // Keep it simple, just ask for a name

        [Range (1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }

        public string Comment { get; set; }

        // Foreign key linking back to the FoodItem
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
    }
}
