using Fruitables.DAL.Data;
using Fruitables.DAL.Models;
using Fruitables.PL.Areas.DashBoard.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fruitables.PL.Controllers
{
    public class CartController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;

        public CartController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context) : base(userManager, _context)
        {
            this.userManager = userManager;
            context = _context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if(user is null)
            {
                return RedirectToAction("Login", "Accounts", new { area = "Identity" });
            }
            var cart = await context.Carts
                .Include(c => c.CartItem)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

            if (cart == null)
            {
                cart = new Cart { ApplicationUserId = user.Id };
                context.Carts.Add(cart);
                await context.SaveChangesAsync();
            }

            return View(cart);
        }
        public async Task<IActionResult> Add(int Id)
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

            var cartItem = cart.CartItem.FirstOrDefault(ci => ci.ProductId == Id);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = Id,
                    Quantity = 1
                };
                context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
                context.CartItems.Update(cartItem);
            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult decrease(int newQuantityA,int cartItemIdA)
        {
            var count = newQuantityA-1;
            if (count <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }
            var cartItem = context.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemIdA);

            if (cartItem == null)
            {
                return NotFound("CartItem not found.");
            }
            cartItem.Quantity = count;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Increase(int newQuantityB, int cartItemIdB)
        {
            var count = newQuantityB + 1;
            if (count <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }
            var cartItem = context.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemIdB);

            if (cartItem == null)
            {
                return NotFound("CartItem not found.");
            }

            // تحديث الكمية
            cartItem.Quantity = count;

            // حفظ التغييرات
            context.SaveChanges();

            // إعادة توجيه المستخدم إلى صفحة السلة
            return RedirectToAction("Index");
        }
        public IActionResult RemovtItem(int Id)
        {
            var cartItem = context.CartItems.FirstOrDefault(ci => ci.CartItemId == Id);
            context.CartItems.Remove(cartItem);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
