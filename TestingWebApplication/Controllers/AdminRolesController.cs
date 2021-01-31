namespace TestingWebApplication.Controllers
{
    using System.Threading.Tasks;
    using Data.Database.Model;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.AdminRoles;

    /// <summary>
    /// Контроллер методов административной панели для управления ролями.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminRolesController : Controller
    {
        /// <summary>
        /// Менеджер ролей.
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Менеджер пользователей.
        /// </summary>
        private readonly UserManager<UserDto> _userManager;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AdminRolesController"/>.
        /// </summary>
        /// <param name="roleManager">Менеджер ролей.</param>
        /// <param name="userManager">Менеджер пользователей.</param>
        public AdminRolesController(RoleManager<IdentityRole> roleManager, UserManager<UserDto> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Отображает главную страницу контроллера.
        /// </summary>
        /// <returns>Результат для отображения.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Отображает страницу для создания роли.
        /// </summary>
        /// <returns>Результат для отображения.</returns>
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        /// <summary>
        /// Выполняет обработку полученных данных для создания роли.
        /// </summary>
        /// <param name="model">Модель с описанием роли.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
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
                return RedirectToAction("ShowList");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(err.Code, err.Description);
            }

            return View(model);
        }

        /// <summary>
        /// Отображает страницу списка ролей.
        /// </summary>
        /// <returns>Задача, возвращающая результат для отображения.</returns>
        [HttpGet]
        public async Task<IActionResult> ShowList()
        {
            var rolesList = await _roleManager.Roles.ToListAsync().ConfigureAwait(false);

            return View(rolesList);
        }

        /// <summary>
        /// Отображает страницу редактирования роли.
        /// </summary>
        /// <param name="roleId">Идентификатор роли для редактирования.</param>
        /// <returns>Задача, возвращающая результат для отображения.</returns>
        [HttpGet]
        public async Task<IActionResult> EditRole([FromQuery] string roleId)
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

        /// <summary>
        /// Выполняет обработку полученных данных для редактирования роли.
        /// </summary>
        /// <param name="model">Модель с описанием роли.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
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

            return RedirectToAction("ShowList");
        }

        /// <summary>
        /// Выполняет удаление роли.
        /// </summary>
        /// <param name="roleId">Идентификатор роли для удаления.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteRole([FromQuery] string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false);
            if (role == null)
            {
                return StatusCode(404, $"Роль с заданным идентификатором ({roleId}) не найдена.");
            }

            await _roleManager.DeleteAsync(role).ConfigureAwait(false);
            return RedirectToAction("ShowList");
        }
    }
}
