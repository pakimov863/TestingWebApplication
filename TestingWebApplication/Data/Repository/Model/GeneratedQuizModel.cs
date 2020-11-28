namespace TestingWebApplication.Data.Repository.Model
{
    using System;

    /// <summary>
    /// Модель сгенерированного теста.
    /// </summary>
    public class GeneratedQuizModel
    {
        /// <summary>
        /// Получает или задает идентификатор сгенерированного теста.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает дату начала теста.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Получает или задает исходный тест.
        /// </summary>
        public QuizModel SourceQuiz { get; set; }
    }
}
