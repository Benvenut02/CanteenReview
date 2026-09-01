using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CanteenReview.Data;
using CanteenReview.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CanteenReview.Pages.FoodItems
{
    public class CreateModel : PageModel
    {
        private readonly CanteenContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(CanteenContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FoodItem FoodItem { get; set; } = new FoodItem();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("FoodItem.Reviews");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Please upload a valid image file (.jpg, .jpeg, .png, .gif, .webp, .svg).");
                    return Page();
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                FoodItem.ImageUrl = $"/uploads/{uniqueFileName}";
            }
            else if (string.IsNullOrWhiteSpace(FoodItem.ImageUrl))
            {
                FoodItem.ImageUrl = "🍲";
            }

            _context.FoodItems.Add(FoodItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
