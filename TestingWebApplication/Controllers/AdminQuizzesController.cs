namespace TestingWebApplication.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Database;
    using Data.Database.Model;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.AdminQuizzes;

    public class AdminQuizzesController : Controller
    {
        private readonly UserManager<UserDto> _userManager;

        private readonly AppDbContext _dbContext;

        public AdminQuizzesController(UserManager<UserDto> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateQuiz()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz(CreateQuizViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var quizDto = TranslateCreateQuizModel(model);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                ModelState.AddModelError("UnknownUser", "Невозможно определить пользователя в этой сессии");
                return View(model);
            }

            quizDto.Creator = currentUser;

            await _dbContext.Quizzes.AddAsync(quizDto).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("ShowList");
        }

        [HttpGet]
        public async Task<IActionResult> ShowList()
        {
            var quizzesList = await _dbContext.Quizzes
                .Include(e => e.QuizBlocks)
                .ToListAsync().ConfigureAwait(false);

            return View(quizzesList);
        }

        [HttpGet]
        public async Task<IActionResult> EditQuiz(long quizId)
        {
            var quizDto = await _dbContext.Quizzes
                .Include(e => e.QuizBlocks)
                .ThenInclude(e => e.Question)
                .Include(e => e.QuizBlocks)
                .ThenInclude(e => e.Answers)
                .FirstOrDefaultAsync(e => e.Id == quizId).ConfigureAwait(false);
            if (quizDto == null)
            {
                return StatusCode(404, $"Тест с заданным идентификатором ({quizId}) не найден.");
            }

            var model = TranslateEditQuizModel(quizDto);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditQuiz(EditQuizViewModel model)
        {
            var quizDto = await _dbContext.Quizzes
                .Include(e => e.QuizBlocks)
                .ThenInclude(e => e.Question)
                .Include(e => e.QuizBlocks)
                .ThenInclude(e => e.Answers)
                .FirstOrDefaultAsync(e => e.Id == model.QuizId).ConfigureAwait(false);
            if (quizDto == null)
            {
                return StatusCode(404, $"Тест с заданным идентификатором ({model.QuizId}) не найден.");
            }

            quizDto.Title = model.Title;
            quizDto.TotalTimeSecs = model.TotalTimeSecs;

            foreach (var quizBlock in model.QuizBlocks)
            {
                //// Добавляем новые элементы (с ID=0)
                if (quizBlock.BlockId == 0)
                {
                    var newQuizBlockDto = TranslateQuizBlockModel(quizBlock);
                    quizDto.QuizBlocks.Add(newQuizBlockDto);
                    continue;
                }

                //// Редактируем существующие элементы
                var quizBlockDto = quizDto.QuizBlocks.FirstOrDefault(e => e.Id == quizBlock.BlockId);
                if (quizBlockDto == null)
                {
                    return StatusCode(500, $"Что-то пошло не так при редактировании теста ({model.QuizId}) - блок с заданным ID ({quizBlock.BlockId}) не найден.");
                }

                quizBlockDto.Question.QuestionType = quizBlock.Question.QuestionType;
                quizBlockDto.Question.Text = quizBlock.Question.Text;

                foreach (var answerBlock in quizBlock.Answers)
                {
                    //// Добавляем новые элементы (с ID=0)
                    if (answerBlock.BlockId == 0)
                    {
                        var newAnswerBlockDto = TranslateAnswerModel(answerBlock);
                        quizBlockDto.Answers.Add(newAnswerBlockDto);
                        continue;
                    }

                    //// Редактируем существующие элементы
                    var answerBlockDto = quizBlockDto.Answers.FirstOrDefault(e => e.Id == answerBlock.BlockId);
                    if (answerBlockDto == null)
                    {
                        return StatusCode(500, $"Что-то пошло не так при редактировании теста ({model.QuizId}) - блок ответа с заданным ID ({answerBlock.BlockId}) не найден.");
                    }

                    answerBlockDto.AnswerType = answerBlock.AnswerType;
                    answerBlockDto.IsCorrect = answerBlock.IsCorrect;
                    answerBlockDto.Text = answerBlock.Text;
                }

                //// Удаляем удаленные блоки ответов
                var removedAnswerBlocks = quizBlockDto.Answers.Where(e => quizBlock.Answers.All(k => k.BlockId != e.Id)).ToList();
                foreach (var removedAnswerBlockDto in removedAnswerBlocks)
                {
                    quizBlockDto.Answers.Remove(removedAnswerBlockDto);
                    _dbContext.Answers.Remove(removedAnswerBlockDto);
                }
            }

            //// Удаляем удаленные блоки теста
            var removedQuizBlocks = quizDto.QuizBlocks.Where(e => model.QuizBlocks.All(k => k.BlockId != e.Id)).ToList();
            foreach (var removedQuizBlockDto in removedQuizBlocks)
            {
                quizDto.QuizBlocks.Remove(removedQuizBlockDto);
                _dbContext.QuizBlocks.Remove(removedQuizBlockDto);
            }
            
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("ShowList");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteQuiz(long quizId)
        {
            var quizDto = await _dbContext.Quizzes.FirstOrDefaultAsync(e => e.Id == quizId).ConfigureAwait(false);
            if (quizDto == null)
            {
                return StatusCode(404, $"Тест с заданным идентификатором ({quizId}) не найден.");
            }

            _dbContext.Quizzes.Remove(quizDto);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("ShowList");
        }

        private QuizDto TranslateCreateQuizModel(CreateQuizViewModel model)
        {
            var dto = new QuizDto
            {
                Title = model.Title,
                TotalTimeSecs = model.TotalTimeSecs,
                QuizBlocks = model.QuizBlocks.Select(TranslateQuizBlockModel).ToList()
            };

            return dto;
        }

        private EditQuizViewModel TranslateEditQuizModel(QuizDto dto)
        {
            var model = new EditQuizViewModel
            {
                QuizId = dto.Id,
                Title = dto.Title,
                TotalTimeSecs = dto.TotalTimeSecs,
                QuizBlocks = dto.QuizBlocks.Select(TranslateQuizBlockModel).ToList()
            };

            return model;
        }

        private QuizBlockDto TranslateQuizBlockModel(QuizBlockViewModel model)
        {
            var dto = new QuizBlockDto
            {
                Id = model.BlockId,
                Question = TranslateQuestionModel(model.Question),
                Answers = model.Answers.Select(TranslateAnswerModel).ToList()
            };

            return dto;
        }

        private QuizBlockViewModel TranslateQuizBlockModel(QuizBlockDto dto)
        {
            var model = new QuizBlockViewModel
            {
                BlockId = dto.Id,
                Question = TranslateQuestionModel(dto.Question),
                Answers = dto.Answers.Select(TranslateAnswerModel).ToList()
            };

            return model;
        }

        private QuestionBlockDto TranslateQuestionModel(QuestionBlockViewModel model)
        {
            var dto = new QuestionBlockDto
            {
                Id = model.BlockId,
                Text = model.Text,
                QuestionType = model.QuestionType
            };

            return dto;
        }

        private QuestionBlockViewModel TranslateQuestionModel(QuestionBlockDto dto)
        {
            var model = new QuestionBlockViewModel
            {
                BlockId = dto.Id,
                Text = dto.Text,
                QuestionType = dto.QuestionType
            };

            return model;
        }

        private AnswerBlockDto TranslateAnswerModel(AnswerBlockViewModel model)
        {
            var dto = new AnswerBlockDto
            {
                Id = model.BlockId,
                IsCorrect = model.IsCorrect,
                Text = model.Text,
                AnswerType = model.AnswerType
            };

            return dto;
        }

        private AnswerBlockViewModel TranslateAnswerModel(AnswerBlockDto dto)
        {
            var model = new AnswerBlockViewModel
            {
                BlockId = dto.Id,
                IsCorrect = dto.IsCorrect,
                Text = dto.Text,
                AnswerType = dto.AnswerType
            };

            return model;
        }
    }
}
