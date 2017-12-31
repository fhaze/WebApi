using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class AccountLoginRequest
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}