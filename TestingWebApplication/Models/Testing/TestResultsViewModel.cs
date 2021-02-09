namespace TestingWebApplication.Models.Testing
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Модель для передачи данных о результатах теста.
    /// </summary>
    public class TestResultsViewModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TestResultsViewModel"/>.
        /// </summary>
        public TestResultsViewModel()
        {
            QuizBlocks = new List<ResultQuizBlockViewModel>();
        }

        /// <summary>
        /// Получает или задает заголовок теста.
        /// </summary>
        public string TestTitle { get; set; }

        /// <summary>
        /// Получает или задает время начала теста.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Получает или задает количество вопросов.
        /// </summary>
        public int QuestionCount { get; set; }

        /// <summary>
        /// Получает или задает количество правильных ответов.
        /// </summary>
        public int CorrectAnswersCount { get; set; }

        /// <summary>
        /// Получает процент правильных ответов.
        /// </summary>
        public int CorrectAnswersPercent => CorrectAnswersCount * 100 / QuestionCount;

        /// <summary>
        /// Получает или задает коллекцию блоков теста.
        /// </summary>
        public List<ResultQuizBlockViewModel> QuizBlocks { get; set; }
    }
}
