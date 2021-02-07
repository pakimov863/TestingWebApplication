namespace TestingWebApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Data.Database;
    using Data.Database.Model;
    using Data.Shared;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.AdminQuizResults;
    using Utils;

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

            var filteredQuizzesList = showAll
                ? await _dbContext.Quizzes.ToListAsync().ConfigureAwait(false)
                : await _dbContext.Quizzes.Where(e => e.Creator.Id == currentUser.Id).ToListAsync().ConfigureAwait(false);

            var result2 = new List<QuizInfoViewModel>();
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

                result2.Add(model);
            }

            return View(result2);
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

        [HttpPost]
        public async Task<IActionResult> ShowResults(ShowResultsViewModel model)
        {
            await UpdateQuizStatuses().ConfigureAwait(false);

            var generatedQuizIds = model.Answers.Where(e => e.IsSelected).Select(e => e.GeneratedQuizId).ToList();
            var generatedQuizzes = await _dbContext.UserQuizzes
                .Where(e => generatedQuizIds.Contains(e.Id))
                .Include(e => e.RespondentUser)
                .Include(e => e.SourceQuiz)
                .ThenInclude(e => e.QuizBlocks)
                .ThenInclude(e => e.Question)
                .Include(e => e.SourceQuiz)
                .ThenInclude(e => e.QuizBlocks)
                .ThenInclude(e => e.Answers)
                .Include(e => e.UserAnswers)
                .ToListAsync().ConfigureAwait(false);

            var resultSb = new StringBuilder();
            resultSb.Append("Пользователь;").Append("Правильных ответов;").Append("Всего вопросов;").Append("% правильных;").AppendLine();
            foreach (var generatedQuiz in generatedQuizzes)
            {
                var correctAnswerCount = 0;
                foreach (var quizBlock in generatedQuiz.SourceQuiz.QuizBlocks)
                {
                    var quizUserAnswer = generatedQuiz.UserAnswers.FirstOrDefault(e => e.QuizBlockId == quizBlock.Id);
                    if (quizUserAnswer == null)
                    {
                        continue;
                    }

                    if (CommonHelpers.CheckQuizAnswer(quizBlock, quizUserAnswer))
                    {
                        correctAnswerCount++;
                    }
                }

                var correctPercent = correctAnswerCount * 100 / generatedQuiz.SourceQuiz.QuizBlocks.Count;

                 resultSb
                     .Append(generatedQuiz.RespondentUser.Name).Append(";")
                     .Append(correctAnswerCount).Append(";")
                     .Append(generatedQuiz.SourceQuiz.QuizBlocks.Count).Append(";")
                     .Append(correctPercent).Append(";")
                     .AppendLine();
            }

            var fileName = $"QuizResults_{model.SourceQuizId}_{DateTimeOffset.Now.ToUnixTimeSeconds()}.csv";
            var stream = new MemoryStream();
            var csvWriter = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            await csvWriter.WriteAsync(resultSb.ToString()).ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "text/csv", fileName);
        }

        /// <summary>
        /// Отображает страницу с сессией пройденного теста.
        /// </summary>
        /// <param name="sessionId">Идентификатор тестовой сессии с результатами.</param>
        /// <returns>Задача, возвращающая результат для отображения.</returns>
        [HttpGet]
        public async Task<IActionResult> ShowSessionResult([FromQuery] long sessionId)
        {
            var generatedQuiz = await _dbContext.UserQuizzes
                .Where(e => e.Id == sessionId)
                .Include(e => e.RespondentUser)
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
                return StatusCode(404, $"Сессия с заданным идентификатором ({sessionId}) не найдена.");
            }

            var model = TranslateSessionResultModel(generatedQuiz);
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
        /// <returns>Сгенерированная модель.</returns>
        private ShowSessionResultViewModel TranslateSessionResultModel(GeneratedQuizDto dto)
        {
            var model = new ShowSessionResultViewModel();
            model.Title = dto.SourceQuiz.Title;
            model.TotalTimeSecs = dto.SourceQuiz.TotalTimeSecs;
            model.Username = dto.RespondentUser.Name;
            model.StartTime = dto.StartTime;
            model.EndTime = dto.EndTime;

            foreach (var quizBlockDto in dto.SourceQuiz.QuizBlocks)
            {
                var quizBlockModel = new ResultQuizBlockViewModel();
                
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

                model.QuizBlocks.Add(quizBlockModel);
            }

            return model;
        }
    }
}
