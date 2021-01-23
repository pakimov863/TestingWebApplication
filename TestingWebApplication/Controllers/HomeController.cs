namespace TestingWebApplication.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TestingWebApplication.Data.Database;
    using TestingWebApplication.Data.Database.Model;

    /// <summary>
    /// Контроллер методов для домашней страницы.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Менеджер пользователей.
        /// </summary>
        private readonly UserManager<UserDto> _userManager;

        /// <summary>
        /// Менеджер ролей.
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HomeController"/>.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных.</param>
        /// <param name="userManager">Менеджер пользователей.</param>
        /// <param name="roleManager">Менеджер ролей.</param>
        public HomeController(AppDbContext dbContext, UserManager<UserDto> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Отображает главную страницу контроллера.
        /// </summary>
        /// <returns>Результат для отображения.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Выполняет очистку базы данных.
        /// </summary>
        /// <returns>Статус операции для отображения.</returns>
        public string Clear()
        {
            AppDbContextSeeder.Clear(_dbContext);
            AppDbContextSeeder.SeedTesting(_dbContext, _userManager, _roleManager);

            return "Cleared";
        }
    }
}
