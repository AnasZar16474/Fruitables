using System.ComponentModel.DataAnnotations;

namespace Fruitables.PL.Areas.Identity.Models.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
