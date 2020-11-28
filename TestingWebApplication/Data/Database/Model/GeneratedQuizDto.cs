namespace TestingWebApplication.Data.Database.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Описание хранимого сгенерированного теста.
    /// </summary>
    public class GeneratedQuizDto
    {
        /// <summary>
        /// Получает или задает идентификатор сгенерированного теста.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает тег теста.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Получает или задает значение, показывающее, завершен ли тест.
        /// </summary>
        public bool IsEnded { get; set; }

        /// <summary>
        /// Получает или задает время начала тестирования.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Получает или задает тест, к которому привязан сгенерированный тест.
        /// </summary>
        public virtual QuizDto SourceQuiz { get; set; }

        /// <summary>
        /// Получает или задает пользователя, который выполняет сгенерированный тест.
        /// </summary>
        public virtual UserDto RespondentUser { get; set; }

        /// <summary>
        /// Получает или задает коллекцию ответов пользователя.
        /// </summary>
        public virtual IList<UserAnswerDto> UserAnswers { get; set; }
    }
}
