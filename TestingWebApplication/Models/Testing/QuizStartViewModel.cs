namespace TestingWebApplication.Models.Testing
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о созданном тесте.
    /// </summary>
    public class QuizStartViewModel
    {
        /// <summary>
        /// Получает или задает идентификатор теста.
        /// </summary>
        [Required]
        public long QuizId { get; set; }
    }
}
