using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
    public class EditCustomerModel : PageModel
    {
        [BindProperty]
        public CustomerClass Customer { get; set; }

        private readonly AppDbContext _db;
        public EditCustomerModel(AppDbContext db)
        {
            _db = db;
        }

        // Controller to edit customers
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _db.Customer.FindAsync(id);
            if (Customer == null)
            {
                return RedirectToPage("/Admin/AdminCustomer");
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

            _db.Attach(Customer).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
                return RedirectToPage("/Admin/AdminCustomer");
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