namespace Fruitables.PL.Views.ViewModel.ProductDetails
{
    public class CompositeDetailsProductsVM
    {
       public ProductDetailsVM ProductDetailsVM { get; set; }
       public IEnumerable<RelatedProductsVM> RelatedProductsVM { get; set; } 
    }
}
