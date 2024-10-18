using Fruitables.DAL.Data;
using Fruitables.DAL.Models;
using Fruitables.PL.Areas.Identity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fruitables.PL.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountsController(UserManager<ApplicationUser> userManager, ApplicationDbContext Context, SignInManager<ApplicationUser> signInManager,
          RoleManager<IdentityRole> roleManager) {
            this.userManager = userManager;
            context = Context;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Gender = model.Gender,
                Address = model.Address,
                UserName = model.Name,
                Email = model.Email,
                PhoneNumber = model.Phone,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                var cart = new Cart
                {
                    ApplicationUserId=user.Id,
                    CreatedAt = DateTime.Now
                };

                // إضافة السلة إلى قاعدة البيانات
                context.Carts.Add(cart);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(LogIn));
            }
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
               if(User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
                {
                   return RedirectToAction("create", "Products", new { area = "Dashboard" });
                }
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            return View(model);
        }
        public IActionResult UsersView()
        {
            var users = userManager.Users.ToList();
            var DisplayUsers = users.Select(user => new DisplayUsersViewModel()
            {
                City = user.Address,
                Gender = user.Gender ?? "aaaaaaaa",
                Name = user.UserName,
                RoleName = userManager.GetRolesAsync(user).Result
            });

            return View(DisplayUsers);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            IdentityRole Role = new IdentityRole()
            {
                Name = model.RoleName,
            };
            var Result = await roleManager.CreateAsync(Role);
            if (Result.Succeeded)
            {
                return RedirectToAction(nameof(RoleView));
            }

            return View(model);
        }

        public IActionResult RoleView()
        {
            var Roles = roleManager.Roles.ToList();
            var DisplayRoles = Roles.Select(aa => new RoleViewModel()
            {
                RoleName = aa.Name
            }).ToList();
            return View(DisplayRoles);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await roleManager.FindByNameAsync(Id);
            var Results = await roleManager.DeleteAsync(role);
            if (Results.Succeeded)
            {
                return RedirectToAction(nameof(RoleView));
            }
            return View();

        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await userManager.FindByNameAsync(Id);
            var Results = await userManager.DeleteAsync(user);
            if (Results.Succeeded)
            {
                return RedirectToAction(nameof(UsersView));
            }
            return View();
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateUser(string Id)
        {
            var user = await userManager.FindByNameAsync(Id);

            if (user != null)
            {
                DisplayUsersViewModel aa = new DisplayUsersViewModel()
                {
                    Name = user.UserName ?? "Unknow",
                    Gender = user.Gender ?? "Unknow",
                    City = user.Address ?? "Unknow",
                    id = user.Id,
                };
                return View(aa);

            }
            return Content("User Is Null");

        }
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateUser(DisplayUsersViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.id);

            user.Gender = model.Gender;
            user.Address = model.City;
            user.UserName = model.Name;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(UsersView));
            }

            return View(model);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> ChangeRoles(string Id)
        {
            EditRolesViewModel aa = new EditRolesViewModel()
            {
                Id = Id,
                RolesList = roleManager.Roles.Select(
                    role => new SelectListItem
                    {
                        Value = role.Id,
                        Text = role.Name,
                    }
                    ).ToList()
            };

            return View(aa);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeRoles(EditRolesViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Id);
            var roleCurrent = await userManager.GetRolesAsync(user);
            var Results = await userManager.RemoveFromRolesAsync(user, roleCurrent);
            var role = await roleManager.FindByIdAsync(model.selectedRoles);
            var roleNew = await userManager.AddToRoleAsync(user, role.Name);

            return RedirectToAction(nameof(UsersView));
        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(LogIn));
        }
    }
}
