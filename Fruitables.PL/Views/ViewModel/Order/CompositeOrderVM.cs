using Fruitables.DAL.Models;

namespace Fruitables.PL.Views.ViewModel.Order
{
    public class CompositeOrderVM
    {
        public Fruitables.DAL.Models.Order Order { get; set; }
        public Cart Cart { get; set; }
    }
}
