using AutoMapper;
using Fruitables.DAL.Data;
using Fruitables.DAL.Models;
using Fruitables.PL.Areas.DashBoard.Controllers;
using Fruitables.PL.Views.ViewModel.ProductDetails;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fruitables.PL.Controllers
{
    public class ShopDetailsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ShopDetailsController(UserManager<ApplicationUser> userManager,ApplicationDbContext context,IMapper mapper):base(userManager,context) {
            this.userManager = userManager;
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(int? Id)
        {
            if(Id == null)
            {
                Id = 4;
            }
            var product = await context.Products
                .Include(p => p.Reviews)  
                .FirstOrDefaultAsync(p => p.Id == Id);
            var vm = mapper.Map<ProductDetailsVM>(product);
            var relatedProducts = context.Products
              .Where(p => p.Type == product.Type && p.Id != product.Id).ToList();
            var vm1 = mapper.Map<IEnumerable<RelatedProductsVM>>(relatedProducts);
            var reviews = await context.Reviews
                     .Where(r => r.ProductId == Id)
                     .ToListAsync();
            double averageRating = 0;
            if (reviews.Any())
            {
                averageRating = reviews.Average(r => r.Rating);
            }
            ViewBag.AverageRating = averageRating;
            CompositeDetailsProductsVM compositeDetailsProductsVM = new CompositeDetailsProductsVM()
            {
                ProductDetailsVM=vm,
                RelatedProductsVM=vm1,

            };
            return View(compositeDetailsProductsVM);
        }
        public async Task<IActionResult> Add(int productId, int quantity)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToAction("Login", "Accounts", new { area = "Identity" });
            }
            var cart = await context.Carts
                .Include(c => c.CartItem)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

            if (cart == null)
            {
                cart = new Cart { ApplicationUserId = user.Id };
                context.Carts.Add(cart);
                await context.SaveChangesAsync();
            }

            var cartItem = cart.CartItem.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId=cart.CartId,
                    ProductId = productId,
                    Quantity = quantity,
                };

                context.CartItems.Add(cartItem);
            }
            context.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }
        public async Task<IActionResult> AddReview(int Rating,string Comment,int ProductId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToAction("Login", "Accounts", new { area = "Identity" });
            }
             var review = new Review
            {
                ApplicationUserId = user.Id,
                ProductId = ProductId,
                Rating = Rating,
                Text = Comment,
            };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
