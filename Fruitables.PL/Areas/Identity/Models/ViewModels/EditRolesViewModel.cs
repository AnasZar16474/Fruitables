using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fruitables.PL.Areas.Identity.Models.ViewModels
{
    public class EditRolesViewModel
    {
        public string Id { get; set; } = null!;
        public IEnumerable<SelectListItem> RolesList { get; set; } = null!;
        public string? selectedRoles { get; set; }
    }
}
