namespace TestingWebApplication.Models.AdminQuizResults
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о результате сессии.
    /// </summary>
    public class ShowSessionResultViewModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ShowSessionResultViewModel"/>.
        /// </summary>
        public ShowSessionResultViewModel()
        {
            QuizBlocks = new List<ResultQuizBlockViewModel>();
        }

        /// <summary>
        /// Получает или задает название теста.
        /// </summary>
        [Required]
        [Display(Name = "Название теста")]
        public string Title { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя, прошедшего тест.
        /// </summary>
        [Required]
        [Display(Name = "Тест прошел")]
        public string Username { get; set; }

        /// <summary>
        /// Получает или задает время начала тестирования.
        /// </summary>
        [Required]
        [Display(Name = "Время начала теста")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Получает или задает время окончания тестирования.
        /// </summary>
        [Required]
        [Display(Name = "Время окончания теста")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Получает или задает время на прохождение теста в секундах.
        /// </summary>
        [Required]
        [Display(Name = "Время на тест (сек)")]
        public long TotalTimeSecs { get; set; }

        /// <summary>
        /// Получает или задает коллекцию блоков теста.
        /// </summary>
        public List<ResultQuizBlockViewModel> QuizBlocks { get; set; }
    }
}
