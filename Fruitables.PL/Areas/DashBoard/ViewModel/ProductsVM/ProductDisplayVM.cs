namespace Fruitables.PL.Areas.DashBoard.ViewModel.ProductFruitsVM
{
    public class ProductDisplayVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImagName { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
