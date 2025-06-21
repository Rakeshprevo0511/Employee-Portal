using System.ComponentModel.DataAnnotations;

namespace dotnet_core_MVC.Models
{
    public class Employee
    {
        public int Id { get; set; }  // Primary Key

        [Required(ErrorMessage = "Employee Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Employee Name must contain only letters and spaces.")]
        public string EmpName { get; set; }

        [StringLength(60)]
        public string Position { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [Range(18, 65, ErrorMessage = "Age must be between 18 and 65.")]
        public int? Age { get; set; }

        [Range(0, int.MaxValue)]
        public int? Salary { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        // ✅ Phone number validation (10-digit format)
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
