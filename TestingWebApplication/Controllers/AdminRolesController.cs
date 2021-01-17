using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestingWebApplication.Models.AdminRoles;

namespace TestingWebApplication.Controllers
{
    using Data.Database.Model;
    using Microsoft.EntityFrameworkCore;

    public class AdminRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<UserDto> _userManager;

        public AdminRolesController(RoleManager<IdentityRole> roleManager, UserManager<UserDto> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newRole = new IdentityRole { Name = model.RoleName };
            var result = await _roleManager.CreateAsync(newRole).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return RedirectToPage("ShowList");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(err.Code, err.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ShowList()
        {
            var rolesList = await _roleManager.Roles.ToListAsync().ConfigureAwait(false);

            return View(rolesList);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false);
            if (role == null)
            {
                return StatusCode(404, $"Роль с заданным идентификатором ({roleId}) не найдена.");
            }

            var model = new EditRoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };

            foreach (var user in _userManager.Users)
            {
                var isSelected = await _userManager.IsInRoleAsync(user, role.Name).ConfigureAwait(false);

                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = isSelected
                };

                model.UsersInRole.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId).ConfigureAwait(false);
            if (role == null)
            {
                return StatusCode(404, $"Роль с заданным идентификатором ({model.RoleId}) не найдена.");
            }

            role.Name = model.RoleName;
            var roleResult = await _roleManager.UpdateAsync(role).ConfigureAwait(false);
            if (!roleResult.Succeeded)
            {
                foreach (var err in roleResult.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }

                return View(model);
            }

            foreach (var userInRole in model.UsersInRole)
            {
                var user = await _userManager.FindByIdAsync(userInRole.UserId);
                if (userInRole.IsSelected && !await _userManager.IsInRoleAsync(user, role.Name).ConfigureAwait(false))
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userInRole.IsSelected && await _userManager.IsInRoleAsync(user, role.Name).ConfigureAwait(false))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            return RedirectToPage("ShowList");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false);
            if (role == null)
            {
                return StatusCode(404, $"Роль с заданным идентификатором ({roleId}) не найдена.");
            }

            await _roleManager.DeleteAsync(role).ConfigureAwait(false);
            return RedirectToPage("ShowList");
        }
    }
}
