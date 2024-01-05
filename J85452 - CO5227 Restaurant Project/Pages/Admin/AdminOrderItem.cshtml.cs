using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace J85452___CO5227_Restaurant_Project.Pages
{
    public class AdminOrderItemModel : PageModel
    {
        // Set up the order item table so it can be displayed
        private readonly AppDbContext _db;
        public AdminOrderItemModel(AppDbContext db)
        {
            _db = db;
        }

        public IList<OrderItemClass> OrderItem { get; private set; }

        [BindProperty]
        public string Search { get; set; }

        // Return all items in order item table (when the page is loaded)
        public void OnGet()
        {
            OrderItem = _db.OrderItem.FromSqlRaw("SELECT * FROM OrderItem").ToList();
        }

        // Return all items in order item table (when the button "View All Items is clicked")
        public IActionResult OnPostALl()
        {
            // Prevents invalid inputs from being entered
            // The forms are automatically set up to prevent invalid inputs (e.g. by inserting letters in a phone number)
            if (!ModelState.IsValid)
            {
                return Page();
            }

            OrderItem = _db.OrderItem.FromSqlRaw("SELECT * FROM OrderItem").ToList();
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

            OrderItem = _db.OrderItem.FromSqlRaw("SELECT * FROM OrderItem WHERE ItemID LIKE '%" + Search + "%'").ToList();
            return Page();
        }
    }
}
