using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace J85452___CO5227_Restaurant_Project
{
    // Authorisation - prevents any user (other than the admin) from accessing this page
    [Authorize(Roles = "Admin")]
    public class AdminCustomerModel : PageModel
    {
        // Set up the customer table so it can be displayed
        private readonly AppDbContext _db;
        public AdminCustomerModel(AppDbContext db)
        {
            _db = db;
        }

        public IList<CustomerClass> Customer { get; private set; }

        [BindProperty]
        public string Search { get; set; }

        // Return all items in customer table (when the page is loaded)
        public void OnGet()
        {
            Customer = _db.Customer.FromSqlRaw("SELECT * FROM Customer").ToList();
        }

        // Return all items in customer table (when the button "View All Items is clicked")
        public IActionResult OnPostAll()
        {
            // Prevents invalid inputs from being entered
            // The forms are automatically set up to prevent invalid inputs (e.g. by inserting letters in a phone number)
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Customer = _db.Customer.FromSqlRaw("SELECT * FROM Customer").ToList();
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

            Customer = _db.Customer.FromSqlRaw("SELECT * FROM Customer WHERE CustomerName LIKE '%" + Search + "%'").ToList();
            return Page();
        }

        // Delete customers
        public async Task<IActionResult> OnPostDeleteAsync(int customerID)
        {
            var item = await _db.Customer.FindAsync(customerID);
            if (item != null)
            {
                try
                {
                    _db.Customer.Remove(item);
                    await _db.SaveChangesAsync();
                } catch (Exception e)
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