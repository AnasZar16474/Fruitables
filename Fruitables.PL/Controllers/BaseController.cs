using Fruitables.DAL.Data;
using Fruitables.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Fruitables.PL.Areas.DashBoard.Controllers
{
    public class BaseController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public BaseController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext) {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public JsonResult GetCartItemCount()
        {
            int cartItemCount = HttpContext.Session.GetInt32("TotalProduct") ?? 0;
            return Json(new { count = cartItemCount });
        }
        public JsonResult GetTotalPrice()
        {
            var totalPrice = HttpContext.Session.GetString("TotalPrice") ?? "";
            decimal.TryParse(totalPrice, out decimal TotalPrice);
            var FinalPrice = TotalPrice + 3;
            return Json(new { totalPrice = TotalPrice, totalPrice1 = FinalPrice });
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
              context.Result=RedirectToAction("Login", "Accounts", new { area = "Identity" });
                return;
            }
            var cart = await dbContext.Carts
               .Include(c => c.CartItem)
               .ThenInclude(ci => ci.Product)
               .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            var totalPrice = cart.CartItem.Sum(ci => ci.Product.Price * ci.Quantity);
            var location = user.Address;
            int totalProducts = cart.CartItem.Sum(ci => ci.Quantity);
            HttpContext.Session.SetInt32("TotalProduct", totalProducts);
            HttpContext.Session.SetString("Location", location);
            HttpContext.Session.SetString("TotalPrice", totalPrice.ToString());
            if (HttpContext.Session.GetInt32("TotalProduct") != null)
            {
                ViewBag.TotalProducts = (int)HttpContext.Session.GetInt32("TotalProduct");
            }
            else
            {
                ViewBag.TotalProducts = 0; // قيمة افتراضية
            }
            if (HttpContext.Session.GetString("Location") != null)
            {
                ViewBag.Location = (string)HttpContext.Session.GetString("Location");
            }
            else
            {
                ViewBag.Location = ""; // قيمة افتراضية
            }
            if (HttpContext.Session.GetString("TotalPrice") != null)
            {
                string totalPriceString = HttpContext.Session.GetString("TotalPrice");
                decimal.TryParse(totalPriceString, out decimal TotalPrice);
                ViewBag.TotalPrice = TotalPrice;
            }
            else
            {
                ViewBag.TotalProducts = 0; // قيمة افتراضية
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
