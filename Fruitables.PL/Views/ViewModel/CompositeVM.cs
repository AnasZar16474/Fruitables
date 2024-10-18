namespace Fruitables.PL.Views.ViewModel
{
    public class CompositeVM
    {
        public IEnumerable<ProductsVM> ProductFruit { get; set; } = null!;
        public IEnumerable<ProductsVM> ProductVegetable { get; set; } = null!;
    }
}
