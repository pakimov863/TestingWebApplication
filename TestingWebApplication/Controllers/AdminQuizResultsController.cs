namespace TestingWebApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Database;
    using Data.Database.Model;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.AdminQuizResults;

    /// <summary>
    /// Контроллер методов административной панели для просмотра результатов.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminQuizResultsController : Controller
    {
        /// <summary>
        /// Менеджер пользователей.
        /// </summary>
        private readonly UserManager<UserDto> _userManager;

        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AdminQuizzesController"/>.
        /// </summary>
        /// <param name="userManager">Менеджер пользователей.</param>
        /// <param name="dbContext">Контекст базы данных.</param>
        public AdminQuizResultsController(UserManager<UserDto> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
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
        /// Отображает страницу списка тестов.
        /// </summary>
        /// <param name="showAll">Значение, показывающее, что необходимо отобразить все тесты без привязки к текущему пользователю.</param>
        /// <returns>Задача, возвращающая результат для отображения.</returns>
        [HttpGet]
        public async Task<IActionResult> ShowList([FromQuery] bool showAll = false)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            if (currentUser == null)
            {
                return StatusCode(500, "Невозможно определить пользователя в этой сессии.");
            }

            var wherePredicate = showAll
                ? new Func<QuizDto, bool>(e => true)
                : new Func<QuizDto, bool>(e => e.Creator.Id == currentUser.Id);

            var quizzesList = await _dbContext.Quizzes.ToListAsync().ConfigureAwait(false);
            var filteredQuizzesList = quizzesList.Where(wherePredicate);

            var result = new List<QuizInfoViewModel>();
            foreach (var quizDto in filteredQuizzesList)
            {
                var answersCount = await _dbContext.UserQuizzes
                    .Include(e => e.SourceQuiz)
                    .CountAsync(e => e.SourceQuiz.Id == quizDto.Id)
                    .ConfigureAwait(false);

                var model = new QuizInfoViewModel
                {
                    Id = quizDto.Id,
                    Title = quizDto.Title,
                    UserName = quizDto.Creator.Name,
                    AnswersCount = answersCount
                };

                result.Add(model);
            }

            return View(result);
        }

        /// <summary>
        /// Отображает страницу списка результатов для заданного теста.
        /// </summary>
        /// <param name="quizId">Идентификатор теста.</param>
        /// <returns>Задача, возвращающая результат для отображения.</returns>
        [HttpGet]
        public async Task<IActionResult> ShowResults([FromQuery] long quizId)
        {
            var sourceQuiz = await _dbContext.Quizzes
                .Include(e => e.Creator)
                .FirstOrDefaultAsync(e => e.Id == quizId).ConfigureAwait(false);

            var quizResults = await _dbContext.UserQuizzes
                .Include(e => e.SourceQuiz)
                .Include(e => e.RespondentUser)
                .Include(e => e.UserAnswers)
                .ToListAsync().ConfigureAwait(false);
            var filteredQuizResults = quizResults.Where(e => e.SourceQuiz.Id == quizId);

            var result = new ShowResultsViewModel
            {
                SourceQuizId = sourceQuiz.Id,
                SourceQuizTitle = sourceQuiz.Title,
                CreatorUserName = sourceQuiz.Creator.Name
            };

            foreach (var quizResult in filteredQuizResults)
            {
                var model = new GeneratedQuizInfoViewModel
                {
                    GeneratedQuizId = quizResult.Id,
                    Tag = quizResult.Tag,
                    RespondentUserName = quizResult.RespondentUser.Name,
                    StartTime = quizResult.StartTime
                };

                result.Answers.Add(model);
            }

            return View(result);
        }

        /// <summary>
        /// Отображает страницу с сессией пройденного теста.
        /// </summary>
        /// <param name="sessionId">Идентификатор тестовой сессии с результатами.</param>
        /// <returns>Задача, возвращающая результат для отображения.</returns>
        [HttpGet]
        public async Task<IActionResult> ShowSessionResult([FromQuery] long sessionId)
        {
            throw new NotImplementedException();
        }
    }
}
