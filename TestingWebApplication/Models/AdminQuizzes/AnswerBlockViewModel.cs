namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.ComponentModel.DataAnnotations;
    using Data.Shared;

    /// <summary>
    /// Модель для передачи данных о блоке ответа.
    /// </summary>
    public class AnswerBlockViewModel
    {
        /// <summary>
        /// Получает или задает идентификатор блока.
        /// </summary>
        [Display(Name = "Идентификатор блока")]
        public long BlockId { get; set; }

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
        /// Получает или задает тип ответа.
        /// </summary>
        [Required]
        [Display(Name = "Тип ответа")]
        public  AnswerBlockType AnswerType { get; set; }
    }
}
