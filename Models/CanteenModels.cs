using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CanteenReview.Models
{
    public class FoodItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Food item name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Item Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 1000.00, ErrorMessage = "Price must be between 0.01 and 1000.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(50)]
        public string Category { get; set; } = "Snacks";

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Food Image")]
        public string ImageUrl { get; set; } = string.Empty;

        // Navigation property for reviews
        public List<Review> Reviews { get; set; } = new List<Review>();

        [NotMapped]
        public double AverageRating => Reviews != null && Reviews.Any() 
            ? Math.Round(Reviews.Average(r => r.Rating), 1) 
            : 0;

        [NotMapped]
        public int ReviewCount => Reviews?.Count ?? 0;

        [NotMapped]
        public bool IsImageFileOrUrl =>
            !string.IsNullOrWhiteSpace(ImageUrl) &&
            (ImageUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
             ImageUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
             ImageUrl.StartsWith("/", StringComparison.OrdinalIgnoreCase) ||
             ImageUrl.StartsWith("~/ ", StringComparison.OrdinalIgnoreCase) ||
             ImageUrl.StartsWith("data:image", StringComparison.OrdinalIgnoreCase) ||
             ImageUrl.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
             ImageUrl.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
             ImageUrl.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
             ImageUrl.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
             ImageUrl.EndsWith(".webp", StringComparison.OrdinalIgnoreCase) ||
             ImageUrl.EndsWith(".svg", StringComparison.OrdinalIgnoreCase));
    }

    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        [StringLength(100, ErrorMessage = "Student name cannot exceed 100 characters.")]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a star rating.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string? Comment { get; set; }

        [Display(Name = "Submitted On")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign key linking back to the FoodItem
        [Required]
        public int FoodItemId { get; set; }

        public FoodItem? FoodItem { get; set; }
    }
}
