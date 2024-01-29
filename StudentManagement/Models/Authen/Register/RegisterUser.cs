using StudentManagement.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models.Authen.Register
{
    public class RegisterUser : Student
    {
        [Required(ErrorMessage = "Username is required")]

        public string Username { get; set; }

     
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password confirmation is not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number is invalid")]
        public string Phone { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

    }
}
