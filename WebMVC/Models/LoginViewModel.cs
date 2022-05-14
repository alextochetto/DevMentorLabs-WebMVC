using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} required")]
        [Display(Name = "Login")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}