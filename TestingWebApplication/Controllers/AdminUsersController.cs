namespace TestingWebApplication.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Database.Model;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.AdminUsers;

    /// <summary>
    /// Контроллер методов административной панели для управления пользователями.
    /// </summary>
    public class AdminUsersController : Controller
    {
        /// <summary>
        /// Менеджер пользователей.
        /// </summary>
        private readonly UserManager<UserDto> _userManager;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AdminUsersController"/>.
        /// </summary>
        /// <param name="userManager">Менеджер пользователей.</param>
        public AdminUsersController(UserManager<UserDto> userManager)
        {
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
        /// Отображает страницу для создания пользователя.
        /// </summary>
        /// <returns>Результат для отображения.</returns>
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        /// <summary>
        /// Выполняет обработку полученных данных для создания пользователя.
        /// </summary>
        /// <param name="model">Модель с описанием пользователя.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new UserDto
            {
                UserName = model.UserLogin,
                Name = model.UserName,
                CreatedQuizzes = new List<QuizDto>(),
                RespondedQuizzes = new List<GeneratedQuizDto>()
            };

            var result = await _userManager.CreateAsync(user).ConfigureAwait(false);
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
        /// Отображает страницу списка пользователей.
        /// </summary>
        /// <returns>Задача, возвращающая результат для отображения.</returns>
        [HttpGet]
        public async Task<IActionResult> ShowList()
        {
            var usersList = await _userManager.Users.ToListAsync().ConfigureAwait(false);

            return View(usersList);
        }

        /// <summary>
        /// Отображает страницу редактирования пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя для редактирования.</param>
        /// <returns>Задача, возвращающая результат для отображения.</returns>
        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return StatusCode(404, $"Пользователь с заданным идентификатором ({userId}) не найден.");
            }

            var model = new EditUserViewModel
            {
                UserId = user.Id,
                UserName = user.Name,
                UserLogin = user.UserName
            };

            return View(model);
        }

        /// <summary>
        /// Выполняет обработку полученных данных для редактирования пользователя.
        /// </summary>
        /// <param name="model">Модель с описанием пользователя.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId).ConfigureAwait(false);
            if (user == null)
            {
                return StatusCode(404, $"Пользователь с заданным идентификатором ({model.UserId}) не найден.");
            }

            if (!string.IsNullOrWhiteSpace(model.Password) && !string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                await _userManager.RemovePasswordAsync(user).ConfigureAwait(false);
                await _userManager.AddPasswordAsync(user, model.Password).ConfigureAwait(false);
            }

            user.Name = model.UserName;
            user.UserName = model.UserLogin;

            var userResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);
            if (!userResult.Succeeded)
            {
                foreach (var err in userResult.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }

                return View(model);
            }

            return RedirectToAction("ShowList");
        }

        /// <summary>
        /// Выполняет удаление пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя для удаления.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return StatusCode(404, $"Пользователь с заданным идентификатором ({userId}) не найден.");
            }

            if (currentUser.Id == user.Id)
            {
                return StatusCode(403, $"Невозможно удалить пользователя, от имени которого выполняется сессия ({userId}).");
            }

            await _userManager.DeleteAsync(user).ConfigureAwait(false);
            return RedirectToAction("ShowList");
        }
    }
}
