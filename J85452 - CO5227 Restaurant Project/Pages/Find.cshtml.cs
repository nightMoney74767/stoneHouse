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
    public class FindModel : PageModel
    {
        // Set up the restaurant table so it can be displayed
        private readonly AppDbContext _db;
        public FindModel(AppDbContext db)
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
    }
}
