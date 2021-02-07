namespace TestingWebApplication.Models.AdminQuizResults
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о ответе на тест.
    /// </summary>
    public class GeneratedQuizInfoViewModel
    {
        /// <summary>
        /// Получает или задает идентификатор сгенерированного теста.
        /// </summary>
        [Display(Name = "Идентификатор теста")]
        public long GeneratedQuizId { get; set; }
        
        /// <summary>
        /// Получает или задает значение, показывающее, выбран ли этот пункт.
        /// </summary>
        [Display(Name = "Сохранить в отчет")]
        public bool IsSelected { get; set; }

        /// <summary>
        /// Получает или задает тег теста.
        /// </summary>
        [Display(Name = "Тег теста")]
        public string Tag { get; set; }

        /// <summary>
        /// Получает или задает время начала тестирования.
        /// </summary>
        [Display(Name = "Дата начала")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя, прошедшего тест.
        /// </summary>
        [Display(Name = "Тест выполнил")]
        public string RespondentUserName { get; set; }
    }
}
