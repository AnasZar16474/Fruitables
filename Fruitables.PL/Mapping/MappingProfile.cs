using AutoMapper;
using Fruitables.DAL.Models;
using Fruitables.PL.Areas.DashBoard.ViewModel.OrderVM;
using Fruitables.PL.Areas.DashBoard.ViewModel.ProductFruitsVM;
using Fruitables.PL.Views.ViewModel;
using Fruitables.PL.Views.ViewModel.Order;
using Fruitables.PL.Views.ViewModel.ProductDetails;
using Fruitables.PL.Views.ViewModel.Shop;

namespace Fruitables.PL.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductCreateVM, Product>();
            CreateMap<Product, ProductsVM>();
            CreateMap<Product, ProductDisplayVM>();
            CreateMap<Product, ProductDeleteVM>();
            CreateMap<Product, ProductEditVM>().ReverseMap();
            CreateMap<Order, OrderDisplayVM > ();
            CreateMap<Product, ProductAllVM>();
            CreateMap<Product, ProductDetailsVM>();
            CreateMap<Product, RelatedProductsVM>();
            




        }
    }
}
