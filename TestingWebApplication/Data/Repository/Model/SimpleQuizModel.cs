namespace TestingWebApplication.Data.Repository.Model
{
    /// <summary>
    /// Упрощенная модель теста.
    /// </summary>
    public class SimpleQuizModel
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
    }
}
