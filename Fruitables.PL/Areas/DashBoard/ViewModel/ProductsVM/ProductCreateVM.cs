using Fruitables.DAL.Models;

namespace Fruitables.PL.Areas.DashBoard.ViewModel.ProductFruitsVM
{
    public class ProductCreateVM
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImagName { get; set; }
        public IFormFile Image { get; set; } = null!;
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
    }
}
