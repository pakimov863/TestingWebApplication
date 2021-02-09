namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о редактируемом тесте.
    /// </summary>
    public class EditQuizViewModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EditQuizViewModel"/>.
        /// </summary>
        public EditQuizViewModel()
        {
            QuizBlocks = new List<QuizBlockViewModel>();
        }

        /// <summary>
        /// Получает или задает идентификатор теста.
        /// </summary>
        [Required]
        [Display(Name = "Идентификатор теста")]
        public long QuizId { get; set; }

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
        /// Получает или задает максимальное количество тестовых блоков в сгенерированном тесте.
        /// </summary>
        [Required]
        [Display(Name = "Максимальное количество вопросов")]
        public int MaxQuizBlocksCount { get; set; }

        /// <summary>
        /// Получает или задает коллекцию блоков теста.
        /// </summary>
        public List<QuizBlockViewModel> QuizBlocks { get; set; }
    }
}
