using EquipmentMonitoringSystem.Areas.Identity.Data;
using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private AuthDbContext _authDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(AuthDbContext authDbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authDbContext = authDbContext;
        }

        public IActionResult Index()
        {

            var usersdb = _authDbContext.Users.ToList();
            List<UserViewModel> users = new List<UserViewModel>();
            foreach (ApplicationUser item in usersdb)
            {
                UserViewModel user = new UserViewModel()
                {
                    Email = item.Email,
                    FullName = item.FullName,
                    Id = item.Id,
                };
                users.Add(user);
            }
            return View(users);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var rolesList = RolesToSelectedList();
            var model = new UserEditViewModel
            {
                Roles = rolesList,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserEditViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrWhiteSpace(model.Email.Trim()))
                ModelState.AddModelError(nameof(model.Email), "Указано некорректный логин!");
            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrWhiteSpace(model.Password.Trim()))
                ModelState.AddModelError(nameof(model.Password), "Указано некорректный пароль!");
            if (model.RoleId == string.Empty)
                ModelState.AddModelError(nameof(model.RoleId), "Выберите роль");
            if (model.FullName == string.Empty)
                ModelState.AddModelError(nameof(model.RoleId), "Заполните поле с ФИО");

            if (!ModelState.IsValid)
            {
                // Список не передается, поэтому следует получить его
                model.Roles = RolesToSelectedList();
                return View(model);
            }

            ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.Email, FullName = model.FullName };
           /* IdentityUser user = new IdentityUser { Email = model.Email, UserName = model.Email, Fu = model.FullName };*/
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            IdentityRole role = _roleManager.Roles.AsNoTracking().FirstOrDefault(x => x.Id == model.RoleId)!;
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
            return RedirectToAction("Index");
        }

        private List<SelectListItem> RolesToSelectedList()
        {
            var roles = _roleManager.Roles.ToList().Select(role => new SelectListItem
            {
                Value = role.Id.ToString(),
                Text = role.Name,
            }).ToList();

            return roles;
        }
    }
}
