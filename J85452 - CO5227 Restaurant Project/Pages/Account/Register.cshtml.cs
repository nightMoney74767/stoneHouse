using J85452___CO5227_Restaurant_Project.CS;
using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace J85452___CO5227_Restaurant_Project.Pages
{
    public class RegisterModel : PageModel
    {
        // Controller for registering accounts with users
        [BindProperty]
        public Registration Input { get; set; }

        // Baskets
        private AppDbContext _db;
        public CheckoutCustomerClass CheckoutCustomer = new CheckoutCustomerClass();
        public BasketClass Basket = new BasketClass();

        private readonly SignInManager<AppUserClass> _signInManager;
        private readonly UserManager<AppUserClass> _userManager;

        public RegisterModel(UserManager<AppUserClass> userManager, SignInManager<AppUserClass> signInManager, AppDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new AppUserClass { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user,isPersistent: false);

                    // Create a new basket
                    NewBasket();
                    NewCustomer(Input.Email);
                    await _db.SaveChangesAsync();

                    return RedirectToPage("/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("BadRegistrationError", "Registration failure detected");
                }
            } else
            {
                ModelState.AddModelError("BadRegistrationError", "Registration failure detected");
            }
            return Page();
        }

        public void NewBasket()
        {
            var currentBasket = _db.Basket.FromSqlRaw("SELECT * FROM Basket")
                .OrderByDescending(b => b.BasketID)
                .FirstOrDefault();
            if (currentBasket == null)
            {
                Basket.BasketID = 1;
            } else
            {
                Basket.BasketID = currentBasket.BasketID + 1;
            }

            _db.Basket.Add(Basket);
        }

        public void NewCustomer(string Email)
        {
            CheckoutCustomer.Email = Email;
            CheckoutCustomer.BasketID = Basket.BasketID;
            _db.CheckoutCustomer.Add(CheckoutCustomer);
        }

        public void OnGet()
        {
        }
    }
}
