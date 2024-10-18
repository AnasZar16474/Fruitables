namespace Fruitables.PL.Areas.Identity.Models.ViewModels
{
    public class DisplayUsersViewModel
    {
        public string id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string City { get; set; } = null!;
        public IEnumerable<string> RoleName { get; set; } = null!;
    }
}
