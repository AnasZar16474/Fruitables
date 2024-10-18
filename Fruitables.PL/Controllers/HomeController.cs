using AutoMapper;
using Fruitables.DAL.Data;
using Fruitables.DAL.Models;
using Fruitables.PL.Areas.DashBoard.Controllers;
using Fruitables.PL.Models;
using Fruitables.PL.Views.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Fruitables.PL.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager) : base(userManager, dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToAction("Login", "Accounts", new { area = "Identity" });
            }
            var product =await dbContext.Products.Where(P => P.Type == DAL.Models.ProductType.Fruit ).ToListAsync();
            var vm = mapper.Map<IEnumerable<ProductsVM>>(product);
            var productA =await dbContext.Products.Where(P => P.Type == DAL.Models.ProductType.Vegetable).ToListAsync();
            var vmA = mapper.Map<IEnumerable<ProductsVM>>(productA);
            var cart = await dbContext.Carts
               .Include(c => c.CartItem)
                   .ThenInclude(ci => ci.Product)
               .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            int totalProducts = cart.CartItem.Sum(ci => ci.Quantity);
            ViewBag.TotalProducts = totalProducts;
            var model = new CompositeVM
            {
                ProductFruit = vm,
                ProductVegetable=vmA
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
