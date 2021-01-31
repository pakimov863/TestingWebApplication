namespace TestingWebApplication.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TestingWebApplication.Data.Database.Model;
    using TestingWebApplication.Models.Account;

    /// <summary>
    /// Контроллер методов для работы с аккаунтами.
    /// </summary>
    [AllowAnonymous]
    public class AccountController : Controller
    {
        /// <summary>
        /// Менеджер авторизации.
        /// </summary>
        private readonly SignInManager<UserDto> _signInManager;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AccountController"/>.
        /// </summary>
        /// <param name="signInManager">Менеджер авторизации.</param>
        public AccountController(SignInManager<UserDto> signInManager)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// Отображает главную страницу контроллера.
        /// </summary>
        /// <returns>Результат для отображения.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Отображает страницу для авторизации пользователя.
        /// </summary>
        /// <returns>Результат для отображения.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Выполняет обработку полученных данных для авторизации пользователя.
        /// </summary>
        /// <param name="model">Модель с данными пользователя</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, false).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Testing");
            }

            ModelState.AddModelError("Auth", "Неверный логин или пароль.");
            return View(model);
        }

        /// <summary>
        /// Выполняет обработку деавторизации пользователя.
        /// </summary>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
