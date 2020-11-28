namespace TestingWebApplication.Data.Database.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Описание хранимого блока вопроса.
    /// </summary>
    public class QuestionBlockDto
    {
        /// <summary>
        /// Получает или задает идентификатор блока вопроса.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает текст вопроса.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Получает или задает идентификатор связанного блока теста.
        /// </summary>
        public virtual long QuizId { get; set; }

        /// <summary>
        /// Получает или задает связанный блок теста.
        /// </summary>
        public virtual QuizBlockDto Quiz { get; set; }
    }
}
