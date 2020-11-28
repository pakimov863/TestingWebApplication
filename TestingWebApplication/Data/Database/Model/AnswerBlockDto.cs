namespace TestingWebApplication.Data.Database.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Shared;

    /// <summary>
    /// Описание хранимого объекта-ответа.
    /// </summary>
    public class AnswerBlockDto
    {
        /// <summary>
        /// Получает или задает идентификатор.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает текст ответа.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Получает или задает значение, показывающее, является ли ответ правильным.
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Получает или задает тип ответа.
        /// </summary>
        public AnswerBlockType AnswerType { get; set; }

        /// <summary>
        /// Получает или задает блок теста, которому принадлежит этот ответ.
        /// </summary>
        public virtual QuizBlockDto Quiz { get; set; }
    }
}
