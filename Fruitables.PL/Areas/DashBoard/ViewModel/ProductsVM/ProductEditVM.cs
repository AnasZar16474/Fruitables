namespace Fruitables.PL.Areas.DashBoard.ViewModel.ProductFruitsVM
{
    public class ProductEditVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IFormFile Image { get; set; } = null!;
        public string? ImagName { get; set; }
        public decimal Price { get; set; }
    }
}
