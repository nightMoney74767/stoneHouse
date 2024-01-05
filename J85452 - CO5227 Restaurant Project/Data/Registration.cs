using System.ComponentModel.DataAnnotations;
namespace J85452___CO5227_Restaurant_Project.Data
{
    public class Registration
    {
        // User registration model class which stores an email address, password and confirmation password
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password that you entered don't match.")]
        public string ConfirmPassword { get; set; }
    }
}
