using AutoMapper;
using Fruitables.DAL.Data;
using Fruitables.DAL.Models;
using Fruitables.PL.Areas.DashBoard.ViewModel.ProductFruitsVM;
using Fruitables.PL.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fruitables.PL.Areas.DashBoard.Controllers
{
    [Area("DashBoard")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ProductsController(ApplicationDbContext dbContext, IMapper mapper) {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public IActionResult Create()
        {
            ViewBag.ProductTypes = new SelectList(Enum.GetValues(typeof(ProductType)));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCreateVM VM)
        {
            if (!ModelState.IsValid)
            {
                return View(VM);
            }
            VM.ImagName = FilesSetting.UploadFile(VM.Image, "Images");
            var model = mapper.Map<Product>(VM);
            dbContext.Add(model);
            dbContext.SaveChanges();
            return Content("Add Succeded");
        }
        [HttpGet]
        public IActionResult Display()
        {
            var product = dbContext.Products.ToList();
            var vm = mapper.Map<IEnumerable<ProductDisplayVM>>(product);
            return View(vm);
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var product = dbContext.Products.Find(Id);
            if (product is null)
            {
                return RedirectToAction(nameof(Display));
            }
            var vm = mapper.Map<ProductDeleteVM>(product);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletedConfirmed(int Id)
        {
            var product = dbContext.Products.Find(Id);
            if (product is null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                FilesSetting.DeleteFile(product.ImagName, "Images");
                dbContext.Remove(product);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Display));
            }

        }
        public IActionResult Edit(int Id)
        {
            var product = dbContext.Products.Find(Id);
            if (product is null)
            {
                return NotFound();
            }
            else
            {

                return View(mapper.Map<ProductEditVM>(product));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            vm.ImagName = FilesSetting.UploadFile(vm.Image, "Images");
            var product = dbContext.Products.Find(vm.Id);

            if (product is null)
            {
                return NotFound();
            }
            else
            {
                FilesSetting.DeleteFile(product.ImagName, "Images");
                mapper.Map(vm, product);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Display));
            }

        }

    }
}
