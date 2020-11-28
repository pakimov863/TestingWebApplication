namespace TestingWebApplication.Data.Database.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Описание хранимого ответа пользователя.
    /// </summary>
    public class UserAnswerDto
    {
        /// <summary>
        /// Получает или задает идентификатор ответа пользователя.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает идентификатор сгенерированного теста.
        /// </summary>
        public long GeneratedQuizId { get; set; }

        /// <summary>
        /// Получает или задает идентификатор блока теста.
        /// </summary>
        public long QuizBlockId { get; set; }

        /// <summary>
        /// Получает или задает ответ пользователя.
        /// </summary>
        public string UserAnswer { get; set; }

        /// <summary>
        /// Получает или задает связанный сгенерированный тест.
        /// </summary>
        public virtual GeneratedQuizDto LinkedQuiz { get; set; }
    }
}
