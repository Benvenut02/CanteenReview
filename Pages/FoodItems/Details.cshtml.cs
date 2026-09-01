using System;
using System.Linq;
using System.Threading.Tasks;
using CanteenReview.Data;
using CanteenReview.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CanteenReview.Pages.FoodItems
{
    public class DetailsModel : PageModel
    {
        private readonly CanteenContext _context;

        public DetailsModel(CanteenContext context)
        {
            _context = context;
        }

        public FoodItem FoodItem { get; set; } = default!;

        [BindProperty]
        public Review NewReview { get; set; } = new Review();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fooditem = await _context.FoodItems
                .Include(f => f.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fooditem == null)
            {
                return NotFound();
            }

            FoodItem = fooditem;
            NewReview.FoodItemId = fooditem.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAddReviewAsync(int id)
        {
            var fooditem = await _context.FoodItems
                .Include(f => f.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fooditem == null)
            {
                return NotFound();
            }

            FoodItem = fooditem;

            // Remove FoodItem navigation validation since it's populated via FoodItemId FK
            ModelState.Remove("NewReview.FoodItem");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewReview.FoodItemId = id;
            NewReview.CreatedAt = DateTime.Now;

            _context.Reviews.Add(NewReview);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = id });
        }

        public async Task<IActionResult> OnPostDeleteReviewAsync(int reviewId, int foodItemId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id = foodItemId });
        }
    }
}
