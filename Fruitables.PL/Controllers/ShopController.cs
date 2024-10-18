using AutoMapper;
using Fruitables.DAL.Data;
using Fruitables.DAL.Models;
using Fruitables.PL.Areas.DashBoard.Controllers;
using Fruitables.PL.Views.ViewModel.Shop;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fruitables.PL.Controllers
{
    public class ShopController : BaseController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ShopController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,IMapper mapper) : base(userManager, context)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            var Product = context.Products.ToList();
            var vm = mapper.Map<IEnumerable<ProductAllVM>>(Product);
            return View(vm);
        }
    }
}
