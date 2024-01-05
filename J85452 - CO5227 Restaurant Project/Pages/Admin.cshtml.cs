using System;
using System.Collections.Generic;
using System.IO;
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
    public class AdminModel : PageModel
    {
        private AppDbContext _db;
        [BindProperty]
        public OfferClass Offer { get; set; }
        [BindProperty]
        public RestaurantClass Restaurant { get; set; }
        [BindProperty]
        public MenuClass Menu { get; set; }
        public AdminModel(AppDbContext db)
        {
            _db = db;
        }

        // Add new offer (unused)
        /*
        public async Task<IActionResult> OnPostNewOffer()
        {
            // Prevents invalid inputs from being entered
            // The forms are automatically set up to prevent invalid inputs (e.g. by inserting letters in a phone number)
            if (!ModelState.IsValid)
            {
                // Registers an error that is displayed in the CSHTML file
                // Adapted from StackOverflow (Isma, 2017)
                ModelState.AddModelError("NewItemError", "Invalid data for new offer");
                // End of adapted code
                return Page();
            }

            Offer.OfferID = countOfferId();
            _db.Offer.Add(Offer);
            await _db.SaveChangesAsync();
            return Page();
        }
        */

        // Add new restaurant
        public async Task<IActionResult> OnPostNewRestaurant()
        {
            Restaurant.RestaurantID = countRestaurantId();
            try
            {
                // The forms are automatically set up to prevent invalid inputs
                _db.Restaurant.Add(Restaurant);
                await _db.SaveChangesAsync();
            } catch (Exception ex)
            {
                // Registers an error that is displayed in the CSHTML file
                // Adapted from StackOverflow (Isma, 2017)
                ModelState.AddModelError("NewItemError", "Invalid data for new menu item");
                // End of adapted code
                return Page();
            }
            return RedirectToPage("/Admin/AdminRestaurant");
        }

        // Add new menu item
        public async Task<IActionResult> OnPostNewMenuItem()
        {
            // The forms are automatically set up to prevent invalid inputs
            Menu.ItemID = countMenuItemId();
            try
            {
                // Add images
                foreach (var imgFile in Request.Form.Files)
                {
                    MemoryStream ms = new MemoryStream();
                    imgFile.CopyTo(ms);
                    Menu.ItemImage = ms.ToArray();

                    ms.Close();
                    ms.Dispose();
                }

                _db.Menu.Add(Menu);
                await _db.SaveChangesAsync();
            } catch (Exception ex)
            {
                // Registers an error that is displayed in the CSHTML file
                // Adapted from StackOverflow (Isma, 2017)
                ModelState.AddModelError("NewItemError", "Invalid data for new menu item");
                // End of adapted code
                return Page();
            }
            return RedirectToPage("/Admin/AdminMenu");
        }

        // Method to find the ID of the last offer and increases the number by one
        // Created with guidance from StackOverflow (Tripathi, 2013)
        public int countOfferId()
        {
            OfferClass[] offers = _db.Offer.FromSqlRaw("SELECT * FROM Offer").ToArray();
            int count = 0;
            for (int i = 0; i < offers.Length; i++)
            {
                count++;
            }
            OfferClass lastOffer = offers[count - 1];
            int lastID = lastOffer.OfferID;
            return lastID + 1;
        }
        // End of adapted code

        // Method to find the ID of the last restaurant and increases the number by one
        // Created with guidance from StackOverflow (Tripathi, 2013)
        public int countRestaurantId()
        {
            RestaurantClass[] restaurants = _db.Restaurant.FromSqlRaw("SELECT * FROM Restaurant").ToArray();
            int count = 0;
            for (int i = 0; i < restaurants.Length; i++)
            {
                count++;
            }
            RestaurantClass lastRestaurant = restaurants[count - 1];
            int lastID = lastRestaurant.RestaurantID;
            return lastID + 1;
        }
        // End of adapted code

        // Method to find the ID of the last menu item and increases the number by one
        // Created with guidance from StackOverflow (Tripathi, 2013)
        public int countMenuItemId()
        {
            MenuClass[] menuItems = _db.Menu.FromSqlRaw("SELECT * FROM Menu").ToArray();
            int count = 0;
            for (int i = 0; i < menuItems.Length; i++)
            {
                count++;
            }
            MenuClass lastMenuItem = menuItems[count - 1];
            int lastID = lastMenuItem.ItemID;
            return lastID + 1;
        }
        // End of adapted code

        public void OnGet()
        {

        }
    }
}

// Referneces
// Isma. (2017). How to display alert message box in asp.net core mvc controller?. Retrieved from StackOverflow: https://stackoverflow.com/questions/46150202/how-to-display-alert-message-box-in-asp-net-core-mvc-controller
// Tripathi. (2013). How to count the number of rows from sql table in c#?. Retrieved from StackOverflow: https://stackoverflow.com/questions/20160928/how-to-count-the-number-of-rows-from-sql-table-in-c
