namespace TestingWebApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Database;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.Testing;
    using Utils;

    /// <summary>
    /// Контроллер с методами для тестирования.
    /// </summary>
    [Authorize]
    public class TestingController : Controller
    {
        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly AppDbContext _db;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="TestingController"/>.
        /// </summary>
        /// <param name="db">Контекст базы данных.</param>
        public TestingController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Открывает главную страницу. Так как главная страница тут не нужна - выполняет перенаправление на страницу авторизации.
        /// </summary>
        /// <returns>Результат обработки.</returns>
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Отображает страницу регистрации на тест.
        /// </summary>
        /// <returns>Результат обработки.</returns>
        public ViewResult Login()
        {
            return View();
        }

        /// <summary>
        /// Отображает страницу тестирования.
        /// </summary>
        /// <param name="token">Токен-идентификатор теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        public async Task<IActionResult> Test(string token)
        {
            var generatedQuiz = await _db.UserQuizzes
                .Where(e => e.Tag == token)
                .Include(e => e.SourceQuiz)
                .ThenInclude(e => e.QuizBlocks)
                .ThenInclude(e => e.Question)
                .Include(e => e.SourceQuiz)
                .ThenInclude(e => e.QuizBlocks)
                .ThenInclude(e => e.Answers)
                .FirstOrDefaultAsync();

            if (generatedQuiz == null)
            {
                return StatusCode(404, $"Тест с заданным идентификатором ({token}) не найден.");
            }

            if (DateTime.Now > generatedQuiz.StartTime.Add(TimeSpan.FromSeconds(generatedQuiz.SourceQuiz.TotalTimeSecs))
                || generatedQuiz.IsEnded)
            {
                return RedirectToAction("Results", new {token});
            }

            var quiz = Translation.Translate(generatedQuiz);
            quiz = CommonHelpers.ShuffleQuizData(quiz);

            return View(quiz);
        }

        /// <summary>
        /// Отображает страницу с результатами тестирования.
        /// </summary>
        /// <param name="token">Токен-идентификатор теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        public async Task<IActionResult> Results(string token)
        {
            var generatedQuiz = await _db.UserQuizzes
                .Where(e => e.Tag == token)
                .Include(e => e.SourceQuiz)
                .ThenInclude(e => e.QuizBlocks)
                .ThenInclude(e => e.Question)
                .Include(e => e.SourceQuiz)
                .ThenInclude(e => e.QuizBlocks)
                .ThenInclude(e => e.Answers)
                .Include(e => e.UserAnswers)
                .FirstOrDefaultAsync();

            if (generatedQuiz == null)
            {
                return StatusCode(404, $"Тест с заданным идентификатором ({token}) не найден.");
            }

            var generatedQuizIsEnded = DateTime.Now > generatedQuiz.StartTime.Add(TimeSpan.FromSeconds(generatedQuiz.SourceQuiz.TotalTimeSecs))
                                       || generatedQuiz.IsEnded;
            if (!generatedQuizIsEnded)
            {
                return RedirectToAction("Test", new {token});
            }

            var view = new TestResultsViewModel
            {
                TestTitle = generatedQuiz.SourceQuiz.Title,
                StartTime = generatedQuiz.StartTime,
                QuestionCount =  generatedQuiz.SourceQuiz.QuizBlocks.Count,
                CorrectAnswersCount = 0,
            };

            foreach (var quizBlock in generatedQuiz.SourceQuiz.QuizBlocks)
            {
                var quizUserAnswer = generatedQuiz.UserAnswers.FirstOrDefault(e => e.QuizBlockId == quizBlock.Id);
                if (quizUserAnswer == null)
                {
                    continue;
                }

                if (CommonHelpers.CheckQuizAnswer(quizBlock, quizUserAnswer))
                {
                    view.CorrectAnswersCount++;
                }
            }

            return View(view);
        }
    }
}