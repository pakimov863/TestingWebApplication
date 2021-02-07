namespace TestingWebApplication.Models.AdminQuizResults
{
    using System.ComponentModel.DataAnnotations;
    using Data.Shared;

    /// <summary>
    /// Модель для передачи данных о блоке ответа.
    /// </summary>
    public class ResultAnswerBlockViewModel
    {
        /// <summary>
        /// Получает или задает текст ответа.
        /// </summary>
        [Required]
        [Display(Name = "Текст ответа")]
        public string Text { get; set; }

        /// <summary>
        /// Получает или задает значение, показывающее корректность ответа.
        /// </summary>
        [Required]
        [Display(Name = "Ответ корректный")]
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Получает или задает значение, показывающее, выбран ли ответ пользователем.
        /// </summary>
        [Required]
        [Display(Name = "Ответ выбран")]
        public bool IsAnswered { get; set; }

        /// <summary>
        /// Получает или задает дополнительный стиль для отображающего тега.
        /// </summary>
        [Required]
        [Display(Name = "Html-стиль для тега")]
        public string AdditionalStyle { get; set; }

        /// <summary>
        /// Получает или задает тип ответа.
        /// </summary>
        [Required]
        [Display(Name = "Тип ответа")]
        public AnswerBlockType AnswerType { get; set; }
    }
}
