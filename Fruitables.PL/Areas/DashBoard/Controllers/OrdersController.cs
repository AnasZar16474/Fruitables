using AutoMapper;
using Fruitables.DAL.Data;
using Fruitables.DAL.Models;
using Fruitables.PL.Areas.DashBoard.ViewModel.OrderVM;
using Fruitables.PL.Areas.DashBoard.ViewModel.ProductFruitsVM;
using Microsoft.AspNetCore.Mvc;

namespace Fruitables.PL.Areas.DashBoard.Controllers
{
    [Area("DashBoard")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public OrdersController(ApplicationDbContext context,IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
          var order=context.Orders.ToList();
            var vm = mapper.Map<IEnumerable<OrderDisplayVM>>(order);
            return View(vm);
        }
        public IActionResult ApplyOrder(int Id)
        {
            var order = context.Orders.Find(Id);
            order.OrderStatus = "Applied";
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CancelledOrder(int Id)
        {
            var order = context.Orders.Find(Id);
            order.OrderStatus = "Cancelled";
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
