namespace TestingWebApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Database;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Utils;

    public class TestingController : Controller
    {
        private AppDbContext _db;

        public TestingController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ViewResult Login()
        {
            return View();
        }

        //http://localhost:10333/Testing/Test?token=2b14023c14f74bc496945d19dba89b55
        //http://localhost:10333/Testing/Test?token=8ba7d6ff815f4934b86d68502f5f1ff7
        //http://localhost:10333/Testing/Test?token=8e775ff00a624c2cb435c061b90419fe
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

            if (DateTime.Now >
                generatedQuiz.StartTime.Add(TimeSpan.FromSeconds(generatedQuiz.SourceQuiz.TotalTimeSecs)))
            {
                return RedirectToAction("Results", new {token});
            }

            var quiz = Translation.Translate(generatedQuiz);
            var rnd = new Random(quiz.StartTime.Millisecond);
            quiz.SourceQuiz.QuizBlocks = quiz.SourceQuiz.QuizBlocks.Shuffle(rnd).ToList();
            foreach (var quizBlock in quiz.SourceQuiz.QuizBlocks)
            {
                quizBlock.Answers = quizBlock.Answers.Shuffle(rnd).ToList();
            }

            return View(quiz);
        }

        public ViewResult Results(string token)
        {
            throw new NotImplementedException();
        }
    }
}
