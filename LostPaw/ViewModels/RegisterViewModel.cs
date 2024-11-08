using System.ComponentModel.DataAnnotations;

namespace LostPaw.ViewModels
{
    public class RegisterViewModel
    {
        
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Fullname { get; set; }
    }
}
