using System.ComponentModel.DataAnnotations;

namespace dotnet_core_MVC.DTO
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }  // Age is treated as password
    }
}
