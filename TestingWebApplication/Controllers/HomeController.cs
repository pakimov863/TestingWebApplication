using Microsoft.AspNetCore.Mvc;

namespace TestingWebApplication.Controllers
{
    using System.Linq;
    using Data.Database;
    using Microsoft.EntityFrameworkCore;

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
