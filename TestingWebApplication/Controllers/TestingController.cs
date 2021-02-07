namespace TestingWebApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Database;
    using Data.Database.Model;
    using Data.Repository.Model;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
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
        /// Менеджер пользователей.
        /// </summary>
        private readonly UserManager<UserDto> _userManager;

        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly AppDbContext _db;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="TestingController"/>.
        /// </summary>
        /// <param name="userManager">Менеджер пользователей.</param>
        /// <param name="db">Контекст базы данных.</param>
        public TestingController(UserManager<UserDto> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        /// <summary>
        /// Открывает главную страницу. Так как главная страница тут не нужна - выполняет перенаправление на страницу авторизации.
        /// </summary>
        /// <returns>Результат обработки.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("QuizStart");
        }

        /// <summary>
        /// Отображает страницу регистрации на тест.
        /// </summary>
        /// <returns>Результат обработки.</returns>
        [HttpGet]
        public IActionResult QuizStart()
        {
            return View();
        }

        /// <summary>
        /// Выполняет создание теста.
        /// </summary>
        /// <param name="model">Информация для создания теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> QuizStart([FromForm] QuizStartViewModel model)
        {
            var requiredQuiz = await _db.Quizzes
                .Where(e => e.Id == model.QuizId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (requiredQuiz == null)
            {
                return StatusCode(404, $"Тест с заданным идентификатором ({model.QuizId}) не найден.");
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                ModelState.AddModelError("UnknownUser", "Невозможно определить пользователя в этой сессии");
                return View(model);
            }

            var generatedQuiz = new GeneratedQuizDto
            {
                Tag = Guid.NewGuid().ToString().Replace("-", string.Empty),
                StartTime = DateTime.Now,
                SourceQuiz = requiredQuiz,
                RespondentUser = currentUser,
                UserAnswers = new List<UserAnswerDto>()
            };

            await _db.UserQuizzes.AddAsync(generatedQuiz).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("QuizProcess", "Testing", new {token = generatedQuiz.Tag});
        }

        /// <summary>
        /// Отображает страницу тестирования.
        /// </summary>
        /// <param name="token">Токен-идентификатор теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpGet]
        public async Task<IActionResult> QuizProcess([FromQuery] string token)
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
        /// Выполняет публикацию теста и сохранение результатов.
        /// </summary>
        /// <param name="model">Информация для создания теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> QuizProcess([FromForm] GeneratedQuizModel model)
        {
            var quizDto = await _db.UserQuizzes
                .Where(e => e.Id == model.Id)
                .Include(e => e.UserAnswers)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (quizDto == null)
            {
                return StatusCode(404, $"Тест с заданным идентификатором ({model.Id}) не найден.");
            }

            quizDto.UserAnswers = new List<UserAnswerDto>();
            quizDto.IsEnded = true;
            quizDto.EndTime = DateTime.Now;

            if (model.SourceQuiz.QuizBlocks != null)
            {
                foreach (var quizBlock in model.SourceQuiz.QuizBlocks)
                {
                    var userAnswerString = "";

                    if (quizBlock.UserAnswer != null)
                    {
                        foreach (var userAnswer in quizBlock.UserAnswer)
                        {
                            if (!string.IsNullOrWhiteSpace(userAnswerString))
                            {
                                userAnswerString += Environment.NewLine;
                            }

                            int userAnswerInt;
                            if (int.TryParse(userAnswer, out userAnswerInt))
                            {
                                var answerBlock = quizBlock.Answers[userAnswerInt];
                                userAnswerString += answerBlock.Id;
                            }
                            else
                            {
                                userAnswerString += userAnswer;
                            }
                        }
                    }

                    quizDto.UserAnswers.Add(new UserAnswerDto
                    {
                        GeneratedQuizId = model.Id,
                        QuizBlockId = quizBlock.Id,
                        UserAnswer = userAnswerString,
                    });
                }
            }

            _db.UserQuizzes.Update(quizDto);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("Results", "Testing", new {token = quizDto.Tag});
        }

        /// <summary>
        /// Отображает страницу с результатами тестирования.
        /// </summary>
        /// <param name="token">Токен-идентификатор теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpGet]
        public async Task<IActionResult> Results([FromQuery] string token)
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
                return RedirectToAction("QuizProcess", new {token});
            }

            if (!generatedQuiz.IsEnded)
            {
                generatedQuiz.IsEnded = true;
                generatedQuiz.EndTime = generatedQuiz.StartTime.AddSeconds(generatedQuiz.SourceQuiz.TotalTimeSecs);
                _db.UserQuizzes.Update(generatedQuiz);
                await _db.SaveChangesAsync().ConfigureAwait(false);
            }

            var view = new TestResultsViewModel
            {
                TestTitle = generatedQuiz.SourceQuiz.Title,
                StartTime = generatedQuiz.StartTime,
                QuestionCount =  generatedQuiz.SourceQuiz.QuizBlocks.Count,
                CorrectAnswersCount = 0
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