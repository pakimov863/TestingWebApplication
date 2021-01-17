namespace TestingWebApplication.Models.TestingApi
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о созданном тесте.
    /// </summary>
    public class TestCreateViewModel
    {
        /// <summary>
        /// Получает или задает идентификатор теста.
        /// </summary>
        [Required]
        public long QuizId { get; set; }
        
        /// <summary>
        /// Получает или задает идентификатор пользователя, выполняющего тест.
        /// </summary>
        [Required]
        public string UserId { get; set; }
        
        /// <summary>
        /// Получает или задает идентификатор группы пользователя, выполняющего тест.
        /// </summary>
        [Required]
        public string UserGroupId { get; set; }
    }
}
