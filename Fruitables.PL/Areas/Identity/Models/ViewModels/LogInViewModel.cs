using System.ComponentModel.DataAnnotations;

namespace Fruitables.PL.Areas.Identity.Models.ViewModels
{
    public class LogInViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
