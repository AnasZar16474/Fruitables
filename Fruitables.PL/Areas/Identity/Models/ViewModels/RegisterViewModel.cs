using System.ComponentModel.DataAnnotations;

namespace Fruitables.PL.Areas.Identity.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Address { get; set; } = null!;

    }
}
