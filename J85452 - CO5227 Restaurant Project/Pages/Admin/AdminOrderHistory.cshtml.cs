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
    public class AdminOrderHistoryModel : PageModel
    {
        // Set up the order history table so it can be displayed
        private readonly AppDbContext _db;
        public AdminOrderHistoryModel(AppDbContext db)
        {
            _db = db;
        }

        public IList<OrderHistoryClass> OrderHistory { get; private set; }

        [BindProperty]
        public string Search { get; set; }

        // Return all items in order history table (when the page is loaded)
        public void OnGet()
        {
            OrderHistory = _db.OrderHistory.FromSqlRaw("SELECT * FROM OrderHistory").ToList();
        }

        // Return all items in order history table (when the button "View All Items is clicked")
        public IActionResult OnPostALl()
        {
            // Prevents invalid inputs from being entered
            // The forms are automatically set up to prevent invalid inputs (e.g. by inserting letters in a phone number)
            if (!ModelState.IsValid)
            {
                return Page();
            }

            OrderHistory = _db.OrderHistory.FromSqlRaw("SELECT * FROM OrderHistory").ToList();
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

            OrderHistory = _db.OrderHistory.FromSqlRaw("SELECT * FROM OrderHistory WHERE CustomerID LIKE '%" + Search + "%'").ToList();
            return Page();
        }

        // Delete order history items
        public async Task<IActionResult> OnPostDeleteAsync(int orderID)
        {
            var item = await _db.OrderHistory.FindAsync(orderID);
            if (item != null)
            {
                try
                {
                    // Delete related records from order items table before deleting the order history record
                    IList<OrderItemClass> orderItems = _db.OrderItem.FromSqlRaw("SELECT * FROM OrderItem WHERE OrderID = {0}", orderID).ToList();
                    foreach (OrderItemClass orderItem in orderItems)
                    {
                        _db.OrderItem.Remove(orderItem);
                    }

                    _db.OrderHistory.Remove(item);
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