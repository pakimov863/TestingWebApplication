namespace TestingWebApplication.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TestingWebApplication.Data.Database.Model;
    using TestingWebApplication.Models.Account;

    public class AccountController : Controller
    {
        private readonly SignInManager<UserDto> _signInManager;

        public AccountController(SignInManager<UserDto> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

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
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("Auth", "Неверный логин или пароль.");
            return View(model);
        }
    }
}
