using System.IO;
using System.Threading.Tasks;
using CanteenReview.Data;
using CanteenReview.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CanteenReview.Pages.FoodItems
{
    public class DeleteModel : PageModel
    {
        private readonly CanteenContext _context;
        private readonly IWebHostEnvironment _environment;

        public DeleteModel(CanteenContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public FoodItem FoodItem { get; set; } = default!;

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fooditem = await _context.FoodItems.FindAsync(id);

            if (fooditem != null)
            {
                FoodItem = fooditem;

                // Clean up uploaded image file if it exists
                if (!string.IsNullOrEmpty(FoodItem.ImageUrl) && FoodItem.ImageUrl.StartsWith("/uploads/"))
                {
                    var filePath = Path.Combine(_environment.WebRootPath, FoodItem.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        try { System.IO.File.Delete(filePath); } catch { }
                    }
                }

                _context.FoodItems.Remove(FoodItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}
