using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using J85452___CO5227_Restaurant_Project.CS;

namespace J85452___CO5227_Restaurant_Project.Pages
{
    public class MenuModel : PageModel
    {
        // Set up the menu table so it can be displayed
        private readonly AppDbContext _db;
        private readonly UserManager<AppUserClass> _userManager;
        public MenuModel(AppDbContext db, UserManager<AppUserClass> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IList<MenuClass> Menu { get; private set; }

        [BindProperty]
        public string Search { get; set; }

        // Return all items in menu table (when the page is loaded)
        public void OnGet()
        {
            Menu =_db.Menu.FromSqlRaw("SELECT * FROM Menu").ToList();
        }

        // Return all items in menu table (when the button "View All Items is clicked")
        public IActionResult OnPostAll()
        {
            Menu = _db.Menu.FromSqlRaw("SELECT * FROM Menu").ToList();
            return Page();
        }

        // Return all items containing the search query
        public IActionResult OnPostSearch()
        {
            Menu = _db.Menu.FromSqlRaw("SELECT * FROM Menu WHERE ItemName LIKE '%" + Search + "%'").ToList();
            return Page();
        }

        // Method for purchasing items and adding to basket
        public async Task<IActionResult> OnPostBuyAsync(int itemID)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                CheckoutCustomerClass customer = await _db.CheckoutCustomer.FindAsync(user.Email);
                var item = _db.BasketItem.FromSqlRaw("SELECT * FROM BasketItem WHERE ItemID = {0} AND BasketID = {1}", itemID, customer.BasketID).ToList().FirstOrDefault();
                if (item == null)
                {
                    BasketItemClass newItem = new BasketItemClass()
                    {
                        BasketID = customer.BasketID,
                        ItemID = itemID,
                        Quantity = 1
                    };

                    _db.BasketItem.Add(newItem);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    item.Quantity = item.Quantity + 1;
                    _db.Attach(item).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }
            } catch (Exception ex)
            {
                ModelState.AddModelError("BasketAddError", "Unable to add an item to basket");
            }

            
            return RedirectToPage();
        }

    }
}
