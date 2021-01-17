namespace TestingWebApplication.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Database.Model;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.AdminUsers;

    public class AdminUsersController : Controller
    {
        private readonly UserManager<UserDto> _userManager;

        public AdminUsersController(UserManager<UserDto> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

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
            var usersList = await _userManager.Users.ToListAsync().ConfigureAwait(false);

            return View(usersList);
        }

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

            return RedirectToPage("ShowList");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
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
            return RedirectToPage("ShowList");
        }
    }
}
