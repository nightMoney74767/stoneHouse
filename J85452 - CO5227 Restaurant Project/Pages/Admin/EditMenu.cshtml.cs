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
    public class EditMenuModel : PageModel
    {
        [BindProperty]
        public MenuClass Menu { get; set; }

        private readonly AppDbContext _db;
        public EditMenuModel(AppDbContext db)
        {
            _db = db;
        }

        // Controller to edit menu items
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Menu = await _db.Menu.FindAsync(id);
            if (Menu == null)
            {
                return RedirectToPage("/Admin/AdminMenu");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
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

            if (!ModelState.IsValid)
            {
                // Registers an error that is displayed in the CSHTML file
                // Adapted from StackOverflow (Isma, 2017)
                ModelState.AddModelError("EditItemError", "Invalid data for database update");
                // End of adapted code
                return Page();
            }

            _db.Attach(Menu).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
                return RedirectToPage("/Admin/AdminMenu");
            } catch (Exception ex)
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