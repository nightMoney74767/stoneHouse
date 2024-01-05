using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace J85452___CO5227_Restaurant_Project.Pages
{
    public class LogoutModel : PageModel
    {
        // Logout functionality
        private readonly SignInManager<AppUserClass> _signInManager;
        public LogoutModel(SignInManager<AppUserClass> signInManager)
        {
            _signInManager = signInManager;
        }


        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
