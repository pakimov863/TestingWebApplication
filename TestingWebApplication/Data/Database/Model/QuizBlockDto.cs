namespace TestingWebApplication.Data.Database.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Описание хранимого блока теста.
    /// </summary>
    public class QuizBlockDto
    {
        /// <summary>
        /// Получает или задает идентификатор блока.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает связанный тест.
        /// </summary>
        public virtual QuizDto Quiz { get; set; }

        /// <summary>
        /// Получает или задает связанный блок вопроса.
        /// </summary>
        public virtual QuestionBlockDto Question { get; set; }

        /// <summary>
        /// Получает или задает коллекцию связанных ответов.
        /// </summary>
        public IList<AnswerBlockDto> Answers { get; set; }
    }
}
