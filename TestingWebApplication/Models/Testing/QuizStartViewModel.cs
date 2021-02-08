namespace TestingWebApplication.Models.Testing
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о созданном тесте.
    /// </summary>
    public class QuizStartViewModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="QuizStartViewModel"/>.
        /// </summary>
        public QuizStartViewModel()
        {
            StartedQuizzes = new List<QuizDescriptionViewModel>();
            AvailableQuizzes = new List<QuizDescriptionViewModel>();
        }

        /// <summary>
        /// Получает или задает идентификатор теста.
        /// </summary>
        [Required]
        [Display(Name = "Выбранный тест")]
        public long QuizId { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя.
        /// </summary>
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        /// <summary>
        /// Получает или задает роль пользователя.
        /// </summary>
        [Display(Name = "Роль пользователя")]
        public string UserRole { get; set; }

        /// <summary>
        /// Получает или задает запущенные тесты.
        /// </summary>
        public List<QuizDescriptionViewModel> StartedQuizzes { get; set; }

        /// <summary>
        /// Получает или задает доступные для прохождения тесты.
        /// </summary>
        public List<QuizDescriptionViewModel> AvailableQuizzes { get; set; }
    }
}
