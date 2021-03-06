﻿namespace TestingWebApplication.Controllers
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
    using Data.Shared;
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
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="TestingController"/>.
        /// </summary>
        /// <param name="userManager">Менеджер пользователей.</param>
        /// <param name="dbContext">Контекст базы данных.</param>
        public TestingController(UserManager<UserDto> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
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
        public async Task<IActionResult> QuizStart()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            if (currentUser == null)
            {
                return StatusCode(500, "Невозможно определить пользователя в этой сессии.");
            }

            var model = new QuizStartViewModel();
            var userRoles = await _userManager.GetRolesAsync(currentUser).ConfigureAwait(false);
            model.UserName = currentUser.Name;
            model.UserRole = string.Join(" / ", userRoles);

            var startedQuizzesCount = await _dbContext.UserQuizzes
                .CountAsync(e => !e.IsEnded && e.RespondentUser.Id == currentUser.Id).ConfigureAwait(false);
            if (startedQuizzesCount > 0)
            {
                var startedQuizzes = await _dbContext.UserQuizzes
                    .Include(e => e.RespondentUser)
                    .Include(e => e.SourceQuiz)
                    .Where(e => !e.IsEnded && e.RespondentUser.Id == currentUser.Id)
                    .ToListAsync().ConfigureAwait(false);

                await UpdateQuizStatuses().ConfigureAwait(false);
                foreach (var quiz in startedQuizzes)
                {
                    var quizModel = new QuizDescriptionViewModel();
                    quizModel.QuizTag = quiz.Tag;
                    quizModel.Title = quiz.SourceQuiz.Title;

                    model.StartedQuizzes.Add(quizModel);
                }
            }

            var allQuizzes = await _dbContext.Quizzes.ToListAsync().ConfigureAwait(false);
            foreach (var quiz in allQuizzes)
            {
                var quizModel = new QuizDescriptionViewModel();
                quizModel.QuizId = quiz.Id;
                quizModel.Title = quiz.Title;

                model.AvailableQuizzes.Add(quizModel);
            }

            return View(model);
        }

        /// <summary>
        /// Выполняет создание теста.
        /// </summary>
        /// <param name="model">Информация для создания теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> QuizStart([FromForm] QuizStartViewModel model)
        {
            var requiredQuiz = await _dbContext.Quizzes
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

            await _dbContext.UserQuizzes.AddAsync(generatedQuiz).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

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
            var generatedQuiz = await _dbContext.UserQuizzes
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
            if (quiz.SourceQuiz.MaxQuizBlocks > 0 && quiz.SourceQuiz.MaxQuizBlocks <= quiz.SourceQuiz.QuizBlocks.Count)
            {
                quiz.SourceQuiz.QuizBlocks = quiz.SourceQuiz.QuizBlocks.Take(quiz.SourceQuiz.MaxQuizBlocks).ToList();
            }

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
            var quizDto = await _dbContext.UserQuizzes
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

            _dbContext.UserQuizzes.Update(quizDto);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

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
            var generatedQuiz = await _dbContext.UserQuizzes
                .Where(e => e.Tag == token)
                .Include(e => e.SourceQuiz)
                .ThenInclude(e => e.QuizBlocks)
                .ThenInclude(e => e.Question)
                .Include(e => e.SourceQuiz)
                .ThenInclude(e => e.QuizBlocks)
                .ThenInclude(e => e.Answers)
                .Include(e => e.UserAnswers)
                .FirstOrDefaultAsync().ConfigureAwait(false);

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
                _dbContext.UserQuizzes.Update(generatedQuiz);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            var model = new TestResultsViewModel();
            model.TestTitle = generatedQuiz.SourceQuiz.Title;
            model.StartTime = generatedQuiz.StartTime;
            model.CorrectAnswersCount = 0;

            if (generatedQuiz.SourceQuiz.MaxQuizBlocksCount > 0 && generatedQuiz.SourceQuiz.MaxQuizBlocksCount <= generatedQuiz.SourceQuiz.QuizBlocks.Count)
            {
                if (generatedQuiz.SourceQuiz.ShowResultsToUser)
                {
                    model.QuizBlocks = TranslateSessionResultModel(generatedQuiz).ToList();
                    var tempQuiz = Translation.Translate(generatedQuiz);
                    tempQuiz = CommonHelpers.ShuffleQuizData(tempQuiz);
                    var quizBlockIds = tempQuiz.SourceQuiz.QuizBlocks.Take(tempQuiz.SourceQuiz.MaxQuizBlocks).Select(e => e.Id).ToList();
                    model.QuizBlocks = model.QuizBlocks.Where(e => quizBlockIds.Contains(e.BlockId)).ToList();
                }

                model.QuestionCount = generatedQuiz.SourceQuiz.MaxQuizBlocksCount;
            }
            else
            {
                model.QuestionCount = generatedQuiz.SourceQuiz.QuizBlocks.Count;
            }

            foreach (var quizBlock in generatedQuiz.SourceQuiz.QuizBlocks)
            {
                var quizUserAnswer = generatedQuiz.UserAnswers.FirstOrDefault(e => e.QuizBlockId == quizBlock.Id);
                if (quizUserAnswer == null)
                {
                    continue;
                }

                if (CommonHelpers.CheckQuizAnswer(quizBlock, quizUserAnswer))
                {
                    model.CorrectAnswersCount++;
                }
            }

            return View(model);
        }

        /// <summary>
        /// Выполняет проверку и обновление статусов завершенных тестов.
        /// </summary>
        /// <returns>Задача, выполняющая обновление статусов тестов.</returns>
        private async Task UpdateQuizStatuses()
        {
            var notEndedQuizzes = await _dbContext.UserQuizzes.Where(e => !e.IsEnded).ToListAsync().ConfigureAwait(false);
            var currentDate = DateTime.Now;
            var mustBeEndedQuizzes = notEndedQuizzes.Where(e => (currentDate - e.StartTime).TotalSeconds > e.SourceQuiz.TotalTimeSecs);
            var mustBeSaved = false;
            foreach (var quiz in mustBeEndedQuizzes)
            {
                quiz.IsEnded = true;
                quiz.EndTime = quiz.StartTime.AddSeconds(quiz.SourceQuiz.TotalTimeSecs);
                _dbContext.Update(quiz);
                mustBeSaved = true;
            }

            if (mustBeSaved)
            {
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Выполняет преобразование результатов теста в модель.
        /// </summary>
        /// <param name="dto">DTO результата тестирования.</param>
        /// <returns>Перечисление моделей блоков теста.</returns>
        private IEnumerable<ResultQuizBlockViewModel> TranslateSessionResultModel(GeneratedQuizDto dto)
        {
            foreach (var quizBlockDto in dto.SourceQuiz.QuizBlocks)
            {
                var quizBlockModel = new ResultQuizBlockViewModel();

                quizBlockModel.BlockId = quizBlockDto.Id;
                quizBlockModel.Question = new ResultQuestionBlockViewModel();
                quizBlockModel.Question.Text = quizBlockDto.Question.Text;
                quizBlockModel.Question.QuestionType = quizBlockDto.Question.QuestionType;

                foreach (var answerBlockDto in quizBlockDto.Answers)
                {
                    var answerBlockModel = new ResultAnswerBlockViewModel();
                    answerBlockModel.Text = answerBlockDto.Text;
                    answerBlockModel.AnswerType = answerBlockDto.AnswerType;
                    answerBlockModel.IsCorrect = answerBlockDto.IsCorrect;
                    answerBlockModel.IsAnswered = false;
                    quizBlockModel.Answers.Add(answerBlockModel);

                    var userAnswerDto = dto.UserAnswers.FirstOrDefault(e => e.QuizBlockId == quizBlockDto.Id);
                    if (userAnswerDto != null)
                    {
                        var userAnswerList = userAnswerDto.UserAnswer.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
                        switch (answerBlockDto.AnswerType)
                        {
                            case AnswerBlockType.Text:
                                var firstUserAnswer = userAnswerList.First().Trim();
                                var firstBlockAnswer = answerBlockDto.Text.Trim();
                                answerBlockModel.IsAnswered = firstBlockAnswer.Equals(firstUserAnswer, StringComparison.InvariantCultureIgnoreCase);
                                answerBlockModel.Text += " / " + firstUserAnswer;
                                break;
                            case AnswerBlockType.Checkbox:
                            case AnswerBlockType.Radio:
                                answerBlockModel.IsAnswered = userAnswerList.Contains(answerBlockDto.Id.ToString());
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(answerBlockDto.AnswerType));
                        }
                    }

                    answerBlockModel.AdditionalStyle = answerBlockModel.IsCorrect == answerBlockModel.IsAnswered
                        ? answerBlockModel.IsCorrect 
                            ? "is-valid"
                            : ""
                        : "is-invalid";
                }

                yield return quizBlockModel;
            }
        }
    }
}