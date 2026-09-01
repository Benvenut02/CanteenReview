using System;
using System.Collections.Generic;
using System.Linq;
using CanteenReview.Models;
using Microsoft.EntityFrameworkCore;

namespace CanteenReview.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CanteenContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Look for any food items
            if (context.FoodItems.Any())
            {
                return; // DB has been seeded
            }

            var foodItems = new List<FoodItem>
            {
                new FoodItem
                {
                    Name = "Crispy Potato Samosa",
                    Price = 20.00m,
                    Category = "Snacks",
                    Description = "Golden fried crispy pastry filled with spiced potato and green peas mix, served with sweet mint chutney.",
                    ImageUrl = "🥟",
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            StudentName = "Aarav Sharma",
                            Rating = 5,
                            Comment = "Best samosas on campus! Always fresh, piping hot, and perfectly spiced.",
                            CreatedAt = DateTime.Now.AddDays(-5)
                        },
                        new Review
                        {
                            StudentName = "Priya Patel",
                            Rating = 4,
                            Comment = "Crunchy crust and chutney is delicious. Great evening snack with tea.",
                            CreatedAt = DateTime.Now.AddDays(-2)
                        }
                    }
                },
                new FoodItem
                {
                    Name = "Grilled Paneer Tikka Sandwich",
                    Price = 75.00m,
                    Category = "Snacks",
                    Description = "Triple layer buttered bread filled with marinated grilled cottage cheese, bell peppers, melted cheese and mint mayo.",
                    ImageUrl = "🥪",
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            StudentName = "Rohan Verma",
                            Rating = 5,
                            Comment = "Super cheesy and filling! Perfect for lunch break when you want something rich.",
                            CreatedAt = DateTime.Now.AddDays(-4)
                        },
                        new Review
                        {
                            StudentName = "Ananya Das",
                            Rating = 5,
                            Comment = "The paneer quality is top notch. Generous portion for ₹75.",
                            CreatedAt = DateTime.Now.AddDays(-1)
                        }
                    }
                },
                new FoodItem
                {
                    Name = "South Indian Masala Dosa",
                    Price = 60.00m,
                    Category = "Meals",
                    Description = "Crispy rice-lentil crepe stuffed with tempered potato masala, served with coconut chutney and hot sambar.",
                    ImageUrl = "🥞",
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            StudentName = "Vikram Iyer",
                            Rating = 4,
                            Comment = "Very authentic taste! Coconut chutney is fresh and coconutty.",
                            CreatedAt = DateTime.Now.AddDays(-6)
                        }
                    }
                },
                new FoodItem
                {
                    Name = "Chilled Cold Coffee with Ice Cream",
                    Price = 50.00m,
                    Category = "Beverages",
                    Description = "Rich blended espresso coffee with chilled full-cream milk, topped with a scoop of vanilla ice cream and chocolate drizzle.",
                    ImageUrl = "🧋",
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            StudentName = "Sneha Kulkarni",
                            Rating = 5,
                            Comment = "Literally saves me during afternoon lectures! Creamy and refreshing.",
                            CreatedAt = DateTime.Now.AddDays(-3)
                        },
                        new Review
                        {
                            StudentName = "Devansh Gupta",
                            Rating = 4,
                            Comment = "Sweetness is just right, love the vanilla scoop on top.",
                            CreatedAt = DateTime.Now.AddHours(-12)
                        }
                    }
                },
                new FoodItem
                {
                    Name = "Veg Cheese Loaded Burger",
                    Price = 85.00m,
                    Category = "Snacks",
                    Description = "Crispy vegetable patty in soft sesame bun layered with cheddar slice, lettuce, onions, tomato and secret burger sauce.",
                    ImageUrl = "🍔",
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            StudentName = "Karan Singh",
                            Rating = 4,
                            Comment = "Crispy patty and nice sauce. Value for money!",
                            CreatedAt = DateTime.Now.AddDays(-7)
                        }
                    }
                },
                new FoodItem
                {
                    Name = "Special Cutting Masala Chai",
                    Price = 15.00m,
                    Category = "Beverages",
                    Description = "Aromatic tea brewed with fresh ginger, cardamom, cloves and buffalo milk. Served hot.",
                    ImageUrl = "☕",
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            StudentName = "Meera Nair",
                            Rating = 5,
                            Comment = "The ultimate stress buster during exams! Strong ginger flavor.",
                            CreatedAt = DateTime.Now.AddDays(-2)
                        }
                    }
                }
            };

            context.FoodItems.AddRange(foodItems);
            context.SaveChanges();
        }
    }
}
