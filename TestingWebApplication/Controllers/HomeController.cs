namespace TestingWebApplication.Controllers
{
    using Data.Database;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login", "Testing");
        }

        public string Clear()
        {
            AppDbContextSeeder.Clear(_dbContext);
            AppDbContextSeeder.Seed(_dbContext);

            return "Cleared";
        }
    }
}
