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

            var generatedQuiz = new GeneratedQuizDto
            {
                Tag = Guid.NewGuid().ToString().Replace("-", string.Empty),
                StartTime = DateTime.Now,
                SourceQuiz = requiredQuiz,
                UserAnswers = new List<UserAnswerDto>()
            };

            await _db.UserQuizzes.AddAsync(generatedQuiz).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("Test", "Testing", new {token = generatedQuiz.Tag});
        }

        [HttpPost]
        public async Task<IActionResult> QuizPublish([FromForm] GeneratedQuizModel tags)
        {
            throw new NotImplementedException();
        }
    }
}
