using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace J85452___CO5227_Restaurant_Project.Pages
{
    // Authorisation - prevents any user (other than the admin) from accessing this page
    [Authorize(Roles = "Admin")]
    public class AdminRestaurantModel : PageModel
    {
        // Set up the restaurant table so it can be displayed
        private readonly AppDbContext _db;
        public AdminRestaurantModel(AppDbContext db)
        {
            _db = db;
        }

        public IList<RestaurantClass> Restaurant { get; set; }

        [BindProperty]
        public string Search { get; set; }


        public void OnGet()
        {
            Restaurant = (IList<RestaurantClass>)_db.Restaurant.FromSqlRaw("SELECT * FROM Restaurant").ToList();
        }

        // Return all items in restaurant table (when the button "View All Items is clicked")
        public IActionResult OnPostAll()
        {
            // Prevents invalid inputs from being entered
            // The forms are automatically set up to prevent invalid inputs (e.g. by inserting letters in a phone number)
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Restaurant = _db.Restaurant.FromSqlRaw("SELECT * FROM Restaurant").ToList();
            return Page();
        }

        // Return all items containing the search query
        public IActionResult OnPostSearch()
        {
            // Prevents invalid inputs from being entered
            // The forms are automatically set up to prevent invalid inputs (e.g. by inserting letters in a phone number)
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Restaurant = _db.Restaurant.FromSqlRaw("SELECT * FROM Restaurant WHERE RestaurantName LIKE '%" + Search + "%'").ToList();
            return Page();
        }

        // Delete restaurants
        public async Task<IActionResult> OnPostDeleteAsync(int restaurantID)
        {
            var item = await _db.Restaurant.FindAsync(restaurantID);
            if (item != null)
            {
                try
                {
                    _db.Restaurant.Remove(item);
                    await _db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    // Registers an error that is displayed in the CSHTML file
                    // Adapted from StackOverflow (Isma, 2017)
                    ModelState.AddModelError("DeleteItemError", "Deletion of record failed");
                    // End of adapted code
                    return RedirectToPage();
                }
            }
            return RedirectToPage();
        }
    }
}

// Referneces
// Isma. (2017). How to display alert message box in asp.net core mvc controller?. Retrieved from StackOverflow: https://stackoverflow.com/questions/46150202/how-to-display-alert-message-box-in-asp-net-core-mvc-controller