namespace TestingWebApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Database;
    using Data.Database.Model;
    using Data.Repository.Model;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.TestingApi;

    /// <summary>
    /// Контроллер для API-методов контроллера тестирования.
    /// </summary>
    public class TestingApiController : Controller
    {
        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly AppDbContext _db;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TestingApiController"/>.
        /// </summary>
        /// <param name="db">Контекст базы данных.</param>
        public TestingApiController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Выполняет создание теста.
        /// </summary>
        /// <param name="model">Информация для создания теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> QuizCreate([FromForm] TestCreateViewModel model)
        {
            var requiredQuiz = await _db.Quizzes
                .Where(e => e.Id == model.QuizId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (requiredQuiz == null)
            {
                return StatusCode(404, $"Тест с заданным идентификатором ({model.QuizId}) не найден.");
            }

            var requiredUser = await _db.Users
                .Where(e => e.Id == model.UserId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (requiredUser == null)
            {
                return StatusCode(404, $"Пользователь с заданным идентификатором ({model.UserId}) не найден.");
            }

            var generatedQuiz = new GeneratedQuizDto
            {
                Tag = Guid.NewGuid().ToString().Replace("-", string.Empty),
                StartTime = DateTime.Now,
                SourceQuiz = requiredQuiz,
                RespondentUser = requiredUser,
                UserAnswers = new List<UserAnswerDto>()
            };

            await _db.UserQuizzes.AddAsync(generatedQuiz).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("Test", "Testing", new {token = generatedQuiz.Tag});
        }

        /// <summary>
        /// Выполняет публикацию теста и сохранение результатов.
        /// </summary>
        /// <param name="model">Информация для создания теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> QuizPublish([FromForm] GeneratedQuizModel model)
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
    }
}
