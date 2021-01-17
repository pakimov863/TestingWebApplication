namespace TestingWebApplication.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TestingWebApplication.Data.Database;
    using TestingWebApplication.Data.Database.Model;

    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        private readonly UserManager<UserDto> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(AppDbContext dbContext, UserManager<UserDto> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        public string Clear()
        {
            AppDbContextSeeder.Clear(_dbContext);
            AppDbContextSeeder.SeedTesting(_dbContext, _userManager, _roleManager);

            return "Cleared";
        }
    }
}
