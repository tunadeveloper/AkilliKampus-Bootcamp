using System.ComponentModel.DataAnnotations;

// RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace Bootcamp.PresentationLayer.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string NameSurname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Cinsiyet seçimi zorunludur.")]
        public string Gender { get; set; }
    }
}

