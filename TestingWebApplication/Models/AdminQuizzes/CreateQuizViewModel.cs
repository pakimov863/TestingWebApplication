namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о создаваемом тесте.
    /// </summary>
    public class CreateQuizViewModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CreateQuizViewModel"/>.
        /// </summary>
        public CreateQuizViewModel()
        {
            QuizBlocks = new List<QuizBlockViewModel>();
        }

        /// <summary>
        /// Получает или задает название теста.
        /// </summary>
        [Required]
        [Display(Name = "Название теста")]
        public string Title { get; set; }

        /// <summary>
        /// Получает или задает время на прохождение теста в секундах.
        /// </summary>
        [Required]
        [Display(Name = "Время на тест (сек)")]
        public long TotalTimeSecs { get; set; }

        /// <summary>
        /// Получает или задает коллекцию блоков теста.
        /// </summary>
        public List<QuizBlockViewModel> QuizBlocks { get; set; }
    }
}
