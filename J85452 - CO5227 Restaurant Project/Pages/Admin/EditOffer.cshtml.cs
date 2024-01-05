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
    public class EditOfferModel : PageModel
    {
        [BindProperty]
        public OfferClass Offer { get; set; }

        private readonly AppDbContext _db;
        public EditOfferModel(AppDbContext db)
        {
            _db = db;
        }

        // Controller to edit offers
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Offer = await _db.Offer.FindAsync(id);
            if (Offer == null)
            {
                return RedirectToPage("/Admin/AdminOffer");
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

            _db.Attach(Offer).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
                return RedirectToPage("/Admin/AdminOffer");
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