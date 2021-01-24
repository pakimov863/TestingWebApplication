namespace TestingWebApplication.Models.AdminQuizResults
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных об ответах на тест.
    /// </summary>
    public class ShowResultsViewModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ShowResultsViewModel"/>.
        /// </summary>
        public ShowResultsViewModel()
        {
            Answers = new List<GeneratedQuizInfoViewModel>();
        }

        /// <summary>
        /// Получает или задает идентификатор исходного теста.
        /// </summary>
        [Display(Name = "Идентификатор исходного теста")]
        public long SourceQuizId { get; set; }

        /// <summary>
        /// Получает или задает заголовок исходного теста.
        /// </summary>
        [Display(Name = "Название теста")]
        public string SourceQuizTitle { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя, создавшего тест.
        /// </summary>
        [Display(Name = "Тест создал")]
        public string CreatorUserName { get; set; }

        /// <summary>
        /// Получает или задает коллекцию ответов на тест.
        /// </summary>
        public IList<GeneratedQuizInfoViewModel> Answers { get; set; }
    }
}
