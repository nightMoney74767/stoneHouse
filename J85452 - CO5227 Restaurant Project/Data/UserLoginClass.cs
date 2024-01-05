using System.ComponentModel.DataAnnotations;

namespace J85452___CO5227_Restaurant_Project.CS
{
    // Class for user login functionality
    public class UserLoginClass
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
