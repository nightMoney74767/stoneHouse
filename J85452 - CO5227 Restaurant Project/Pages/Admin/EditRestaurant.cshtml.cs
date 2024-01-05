using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace J85452___CO5227_Restaurant_Project.Pages
{
    public class EditRestaurantModel : PageModel
    {
        [BindProperty]
        public RestaurantClass Restaurant { get; set; }

        private readonly AppDbContext _db;
        public EditRestaurantModel(AppDbContext db)
        {
            _db = db;
        }

        // Controller to edit menu items
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Restaurant = await _db.Restaurant.FindAsync(id);
            if (Restaurant == null)
            {
                return RedirectToPage("/Admin/AdminRestaurant");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                // Registers an error that is displayed in the CSHTML file
                // Adapted from StackOverflow (Isma, 2017)
                ModelState.AddModelError("EditItemError", "Invalid data for database update");
                // End of adapted code
                return Page();
            }

            _db.Attach(Restaurant).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
                return RedirectToPage("/Admin/AdminRestaurant");
            }
            catch (Exception ex)
            {
                // Registers an error that is displayed in the CSHTML file
                // Adapted from StackOverflow (Isma, 2017)
                ModelState.AddModelError("EditItemError", "Invalid data for database update");
                // End of adapted code
            }
            return RedirectToPage();
        }
    }
}