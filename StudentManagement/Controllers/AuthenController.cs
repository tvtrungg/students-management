using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using StudentManagement.Models.Authen.Register;
using StudentManagement.Models.Entities;
using Azure;

namespace StudentManagement.Controllers
{
    public class AuthenController : Controller
    {
        private readonly UserManager<Student> _userManager;
        private readonly RoleManager<Student> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenController(UserManager<Student> userManager, RoleManager<Student> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            // Check user exist
            var user = await _userManager.FindByNameAsync(registerUser.Username);
            if (user != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            // Create user
            user = new Student
            {
                Id = Guid.NewGuid(),
                Username = registerUser.Username,
                Name = registerUser.Name,
                Phone = registerUser.Phone,
                Email = registerUser.Email,
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            
        }
    }
}
