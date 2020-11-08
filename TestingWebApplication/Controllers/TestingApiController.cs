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

    public class TestingApiController : Controller
    {
        private AppDbContext _db;

        public TestingApiController(AppDbContext db)
        {
            _db = db;
        }

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
                .Where(e => string.IsNullOrWhiteSpace(e.Password))
                .Where(e => e.UserName == model.Username)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (requiredUser == null)
            {
                requiredUser = new UserDto
                {
                    UserName = model.Username.Trim(),
                };

                await _db.Users.AddAsync(requiredUser).ConfigureAwait(false);
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

            foreach (var quizBlock in model.SourceQuiz.QuizBlocks)
            {
                var userAnswerString = "";

                foreach (var userAnswer in quizBlock.UserAnswer)
                {
                    int userAnswerInt;
                    if (!int.TryParse(userAnswer, out userAnswerInt))
                    {
                        continue;
                    }

                    var answerBlock = quizBlock.Answers[userAnswerInt];
                    if (!string.IsNullOrWhiteSpace(userAnswerString))
                    {
                        userAnswerString += Environment.NewLine;
                    }

                    userAnswerString += answerBlock.Id;
                }

                quizDto.UserAnswers.Add(new UserAnswerDto
                {
                    GeneratedQuizId = model.Id,
                    QuizBlockId = quizBlock.Id,
                    UserAnswer = userAnswerString,
                });
            }

            _db.UserQuizzes.Update(quizDto);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("Results", "Testing", new {token = quizDto.Tag});
        }
    }
}
