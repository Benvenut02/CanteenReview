using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanteenReview.Data;
using CanteenReview.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CanteenReview.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CanteenContext _context;

        public IndexModel(CanteenContext context)
        {
            _context = context;
        }

        public IList<FoodItem> FoodItems { get; set; } = new List<FoodItem>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedCategory { get; set; }

        public List<string> Categories { get; set; } = new List<string>();

        public int TotalItems { get; set; }
        public int TotalReviews { get; set; }
        public double OverallAvgRating { get; set; }

        public async Task OnGetAsync()
        {
            // Distinct categories list
            Categories = await _context.FoodItems
                .Select(f => f.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .ToListAsync();

            var query = _context.FoodItems
                .Include(f => f.Reviews)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(f => f.Name.Contains(SearchTerm) || (f.Description != null && f.Description.Contains(SearchTerm)));
            }

            if (!string.IsNullOrWhiteSpace(SelectedCategory) && SelectedCategory != "All")
            {
                query = query.Where(f => f.Category == SelectedCategory);
            }

            FoodItems = await query.ToListAsync();

            // Summary Stats
            var allItems = await _context.FoodItems.Include(f => f.Reviews).ToListAsync();
            TotalItems = allItems.Count;
            var allReviews = allItems.SelectMany(f => f.Reviews).ToList();
            TotalReviews = allReviews.Count;
            OverallAvgRating = allReviews.Any() ? Math.Round(allReviews.Average(r => r.Rating), 1) : 0;
        }
    }
}
