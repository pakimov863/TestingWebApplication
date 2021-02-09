namespace TestingWebApplication.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Data.Database;
    using Data.Database.Model;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.AdminQuizzes;

    /// <summary>
    /// Контроллер методов административной панели для управления тестами.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminQuizzesController : Controller
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
        public AdminQuizzesController(UserManager<UserDto> userManager, AppDbContext dbContext)
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
        /// Отображает страницу для создания теста.
        /// </summary>
        /// <returns>Результат для отображения.</returns>
        [HttpGet]
        public IActionResult CreateQuiz()
        {
            return View();
        }

        /// <summary>
        /// Выполняет обработку полученных данных для создания теста.
        /// </summary>
        /// <param name="model">Модель с описанием теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateQuiz(CreateQuizViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var quizDto = TranslateCreateQuizModel(model);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            if (currentUser == null)
            {
                ModelState.AddModelError("UnknownUser", "Невозможно определить пользователя в этой сессии");
                return View(model);
            }

            quizDto.Creator = currentUser;

            await _dbContext.Quizzes.AddAsync(quizDto).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("ShowList");
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

            var quizzesList = await _dbContext.Quizzes
                .Include(e => e.QuizBlocks)
                .ToListAsync().ConfigureAwait(false);

            var filteredList = quizzesList
                .Where(wherePredicate)
                .ToList();

            return View(filteredList);
        }

        /// <summary>
        /// Выполняет экспорт теста.
        /// </summary>
        /// <param name="quizId">Идентификатор теста для экспорта.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpGet]
        public async Task<IActionResult> ExportQuiz([FromQuery] long quizId)
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

            var exportingQuizModel = TranslateCreateQuizModel(quizDto);

            var fileName = $"ExportedQuiz_{quizId}_{DateTimeOffset.Now.ToUnixTimeSeconds()}.json";
            var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, exportingQuizModel).ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/json", fileName);
        }

        /// <summary>
        /// Отображает страницу редактирования теста.
        /// </summary>
        /// <param name="quizId">Идентификатор теста для редактирования.</param>
        /// <returns>Задача, возвращающая результат для отображения.</returns>
        [HttpGet]
        public async Task<IActionResult> EditQuiz([FromQuery] long quizId)
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

        /// <summary>
        /// Выполняет обработку полученных данных для редактирования теста.
        /// </summary>
        /// <param name="model">Модель с описанием теста.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> EditQuiz(EditQuizViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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

            return RedirectToAction("ShowList");
        }

        /// <summary>
        /// Выполняет удаление теста.
        /// </summary>
        /// <param name="quizId">Идентификатор теста для удаления.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteQuiz([FromQuery] long quizId)
        {
            var quizDto = await _dbContext.Quizzes.FirstOrDefaultAsync(e => e.Id == quizId).ConfigureAwait(false);
            if (quizDto == null)
            {
                return StatusCode(404, $"Тест с заданным идентификатором ({quizId}) не найден.");
            }

            _dbContext.Quizzes.Remove(quizDto);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("ShowList");
        }

        /// <summary>
        /// Отображает страницу импорта теста.
        /// </summary>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpGet]
        public IActionResult ImportQuiz()
        {
            return View();
        }

        /// <summary>
        /// Выполняет импорт файла с тестом.
        /// </summary>
        /// <param name="importFile">Импортируемый файл.</param>
        /// <returns>Задача, возвращающая результат обработки.</returns>
        [HttpPost]
        public async Task<IActionResult> ImportQuiz(ImportQuizViewModel importFile)
        {
            CreateQuizViewModel model;

            try
            {
                var fileStream = new MemoryStream();
                await importFile.File.CopyToAsync(fileStream).ConfigureAwait(false);
                fileStream.Seek(0, SeekOrigin.Begin);
                model = await JsonSerializer.DeserializeAsync<CreateQuizViewModel>(fileStream).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Импортированный файл не может быть распознан как действительный тест. Ошибка: {e.Message}");
            }

            if (model == null)
            {
                return StatusCode(500, "Импортированный файл не может быть распознан как действительный тест.");
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            if (currentUser == null)
            {
                return StatusCode(500, "Невозможно определить пользователя в этой сессии.");
            }

            var quizDto = TranslateCreateQuizModel(model);

            //// Обнуляем тест, чтобы при импорте были созданы новые объекты.
            quizDto.Id = 0;
            quizDto.Creator = currentUser;
            foreach (var quizBlock in quizDto.QuizBlocks)
            {
                quizBlock.Id = 0;
                quizBlock.Question.Id = 0;

                foreach (var quizBlockAnswer in quizBlock.Answers)
                {
                    quizBlockAnswer.Id = 0;
                }
            }

            await _dbContext.Quizzes.AddAsync(quizDto).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            
            return RedirectToAction("ShowList");
        }

        /// <summary>
        /// Выполняет трансляцию модели создания теста в DTO.
        /// </summary>
        /// <param name="model">Модель с описанием теста.</param>
        /// <returns>DTO-объект теста.</returns>
        private QuizDto TranslateCreateQuizModel(CreateQuizViewModel model)
        {
            var dto = new QuizDto
            {
                Title = model.Title,
                TotalTimeSecs = model.TotalTimeSecs,
                MaxQuizBlocksCount = model.MaxQuizBlocksCount,
                QuizBlocks = model.QuizBlocks.Select(TranslateQuizBlockModel).ToList()
            };

            return dto;
        }

        /// <summary>
        /// Выполняет трансляцию DTO в модель создания теста.
        /// </summary>
        /// <param name="dto">DTO-объект теста.</param>
        /// <returns>Модель с описанием теста.</returns>
        private CreateQuizViewModel TranslateCreateQuizModel(QuizDto dto)
        {
            var model = new CreateQuizViewModel
            {
                Title = dto.Title,
                TotalTimeSecs = dto.TotalTimeSecs,
                MaxQuizBlocksCount = dto.MaxQuizBlocksCount,
                QuizBlocks = dto.QuizBlocks.Select(TranslateQuizBlockModel).ToList()
            };

            return model;
        }

        /// <summary>
        /// Выполняет трансляцию DTO в модель редактирования теста.
        /// </summary>
        /// <param name="dto">DTO-объект теста.</param>
        /// <returns>Модель с описанием теста.</returns>
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

        /// <summary>
        /// Выполняет трансляцию модели блока теста в DTO.
        /// </summary>
        /// <param name="model">Модель с описанием блока теста.</param>
        /// <returns>DTO-объект блока теста.</returns>
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

        /// <summary>
        /// Выполняет трансляцию DTO в модель блока теста.
        /// </summary>
        /// <param name="dto">DTO-объект блока теста.</param>
        /// <returns>Модель с описанием блока теста.</returns>
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

        /// <summary>
        /// Выполняет трансляцию модели блока вопроса в DTO.
        /// </summary>
        /// <param name="model">Модель с описанием блока вопроса.</param>
        /// <returns>DTO-объект блока вопроса.</returns>
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

        /// <summary>
        /// Выполняет трансляцию DTO в модель блока вопроса.
        /// </summary>
        /// <param name="dto">DTO-объект блока вопроса.</param>
        /// <returns>Модель с описанием блока вопроса.</returns>
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

        /// <summary>
        /// Выполняет трансляцию модели создания теста в DTO.
        /// </summary>
        /// <param name="model">Модель с описанием блока ответа.</param>
        /// <returns>DTO-объект блока ответа.</returns>
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

        /// <summary>
        /// Выполняет трансляцию DTO в модель блока ответа. 
        /// </summary>
        /// <param name="dto">DTO-объект блока ответа.</param>
        /// <returns>Модель с описанием блока ответа.</returns>
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
