namespace TestingWebApplication.Models.Testing
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи описания доступных тестов.
    /// </summary>
    public class QuizDescriptionViewModel
    {
        /// <summary>
        /// Получает или задает идентификатор теста.
        /// </summary>
        [Display(Name = "Идентификатор теста")]
        public long QuizId { get; set; }

        /// <summary>
        /// Получает или задает тег теста.
        /// </summary>
        [Display(Name = "Тег сессии")]
        public string QuizTag { get; set; }

        /// <summary>
        /// Получает или задает заголовок теста.
        /// </summary>
        [Display(Name = "Название теста")]
        public string Title { get; set; }
    }
}
