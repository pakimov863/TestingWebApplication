namespace TestingWebApplication.Data.Repository.Model
{
    using Shared;

    /// <summary>
    /// Модель блока-ответа.
    /// </summary>
    public class AnswerBlockModel
    {
        /// <summary>
        /// Получает или задает идентификатор ответа.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает текст ответа.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Получает или задает тип ответа.
        /// </summary>
        public AnswerBlockType AnswerType { get; set; }

        /// <summary>
        /// Получает или задает значение, показывающее, является ли ответ правильным.
        /// </summary>
        public bool IsCorrect { get; set; }
    }
}
