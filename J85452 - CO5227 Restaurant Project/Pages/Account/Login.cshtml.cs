using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using J85452___CO5227_Restaurant_Project.CS;
using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace J85452___CO5227_Restaurant_Project.Pages.Shared
{
    public class LoginModel : PageModel
    {
        // Controller used for the login process
        [BindProperty]
        public UserLoginClass Input { get; set; }

        private readonly SignInManager<AppUserClass> _signInManager;

        public LoginModel(SignInManager<AppUserClass> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid && Input.Email != null && Input.Password != null)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError("BadLoginError", "Invalid login detected");
                    return Page();
                }
            } else
            {
                ModelState.AddModelError("BadLoginError", "Invalid login detected");
            }
            return Page();
        }

        public void OnGet()
        {
        }
    }
}
