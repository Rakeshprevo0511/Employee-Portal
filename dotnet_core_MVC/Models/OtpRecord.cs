using System.ComponentModel.DataAnnotations;

namespace dotnet_core_MVC.Models
{
    public class OtpRecord
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int EmpId { get; set; }

        [Required]
        public string Email { get; set; }  // User's Email

        [Required]
        public string OTP { get; set; }  // OTP Code

        public DateTime ExpiryTime { get; set; }  // Expiration Time

        public bool IsUsed { get; set; } = false; // To check if OTP is already used
    }
}
