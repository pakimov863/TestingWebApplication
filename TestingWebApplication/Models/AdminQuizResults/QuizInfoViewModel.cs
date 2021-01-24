namespace TestingWebApplication.Models.AdminQuizResults
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о тесте и количестве ответов.
    /// </summary>
    public class QuizInfoViewModel
    {
        /// <summary>
        /// Получает или задает идентификатор теста.
        /// </summary>
        [Display(Name = "Идентификатор теста")]
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает заголовок теста.
        /// </summary>
        [Display(Name = "Название теста")]
        public string Title { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя, создавшего тест.
        /// </summary>
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        
        /// <summary>
        /// Получает или задает количество результатов для теста.
        /// </summary>
        [Display(Name = "Количество ответов")]
        public int AnswersCount { get; set; }
    }
}
