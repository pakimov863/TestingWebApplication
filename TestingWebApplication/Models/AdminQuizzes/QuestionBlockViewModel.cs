namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.ComponentModel.DataAnnotations;
    using Data.Shared;

    /// <summary>
    /// Модель для передачи данных о блоке вопроса.
    /// </summary>
    public class QuestionBlockViewModel
    {
        /// <summary>
        /// Получает или задает идентификатор блока.
        /// </summary>
        [Display(Name = "Идентификатор блока")]
        public long BlockId { get; set; }

        /// <summary>
        /// Получает или задает текст вопроса.
        /// </summary>
        [Required]
        [Display(Name = "Текст вопроса")]
        public string Text { get; set; }
        
        /// <summary>
        /// Получает или задает тип вопроса.
        /// </summary>
        [Required]
        [Display(Name = "Тип вопроса")]
        public QuestionBlockType QuestionType { get; set; }
    }
}
