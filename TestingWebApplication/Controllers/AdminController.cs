namespace TestingWebApplication.Controllers
{
    using Data.Database;
    using Data.Database.Model;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер методов для главной страницы админ-панели.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
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
        public AdminController(AppDbContext dbContext, UserManager<UserDto> userManager, RoleManager<IdentityRole> roleManager)
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
            return View();
        }

        /// <summary>
        /// Выполняет очистку базы данных.
        /// </summary>
        /// <returns>Статус операции для отображения.</returns>
        [HttpGet]
        public string Clear()
        {
            AppDbContextSeeder.Clear(_dbContext);
            AppDbContextSeeder.SeedTesting(_dbContext, _userManager, _roleManager);

            return "OK";
        }
    }
}
