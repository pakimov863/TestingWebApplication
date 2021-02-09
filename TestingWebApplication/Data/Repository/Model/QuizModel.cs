namespace TestingWebApplication.Data.Repository.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Модель теста.
    /// </summary>
    public class QuizModel
    {
        /// <summary>
        /// Получает или задает идентификатор теста.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает заголовок теста.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Получает или задает время на прохождение теста в секундах.
        /// </summary>
        public long TotalTimeSecs { get; set; }

        /// <summary>
        /// Получает или задает максимальное количество тестовых блоков в сгенерированном тесте.
        /// </summary>
        public int MaxQuizBlocks { get; set; }

        /// <summary>
        /// Получает или задает коллекцию вопросов теста.
        /// </summary>
        public IList<QuizBlockModel> QuizBlocks { get; set; }
    }
}
