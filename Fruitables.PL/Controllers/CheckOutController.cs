using AutoMapper;
using Fruitables.DAL.Data;
using Fruitables.DAL.Models;
using Fruitables.PL.Areas.DashBoard.Controllers;
using Fruitables.PL.Views.ViewModel;
using Fruitables.PL.Views.ViewModel.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace Fruitables.PL.Controllers
{
    public class CheckOutController : BaseController
        {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CheckOutController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context,IMapper mapper) : base(userManager, _context)
        {
            this.userManager = userManager;
            context = _context;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
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
            var model = new CompositeOrderVM
            {
                Order=new Order(),
                Cart=cart,
            };

            return View(model);
        }
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var user = await userManager.GetUserAsync(User);
            var userA = context.Users
                    .Include(u => u.cart)                        
                    .ThenInclude(c => c.CartItem)             
                    .ThenInclude(ci => ci.Product)            
                    .FirstOrDefault(u => u.Id == user.Id);

            var cart = await context.Carts
              .Include(c => c.CartItem)
                  .ThenInclude(ci => ci.Product)
              .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
              if (userA == null || userA.cart == null || !userA.cart.CartItem.Any())
            {
                return RedirectToAction("Index", "Home");   
            }
            var orderA = new Order
            {
                ApplicationUserId = user.Id,
                OrderDate = DateTime.Now,
                FirstName = order.FirstName,
                Address = order.Address,
                City = order.City,
                Country = order.Country,
                EmailAddress = order.EmailAddress,
                mobile = order.mobile,
                OrderStatus = "pendding",
                TotalAmount = 0,
                OrderItem = new List<OrderItem>() 
            };
            foreach (var cartItem in userA.cart.CartItem)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price,
                    Order = orderA
                };
                context.OrderItems.Add(orderItem);
                orderA.TotalAmount += orderItem.Quantity * orderItem.Price;
            }
            context.Orders.Add(orderA);
            context.SaveChanges();
            user.cart.CartItem.Clear();
            context.SaveChanges();
            return RedirectToAction("Index","Home");
        }
    }
}
