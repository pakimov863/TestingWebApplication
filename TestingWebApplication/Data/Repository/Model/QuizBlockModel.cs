namespace TestingWebApplication.Data.Repository.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Модель одного блока теста.
    /// </summary>
    public class QuizBlockModel
    {
        /// <summary>
        /// Получает или задает идентификатор блока.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает блок-вопрос.
        /// </summary>
        public QuestionBlockModel Question { get; set; }

        /// <summary>
        /// Получает или задает коллекцию блоков-ответов.
        /// </summary>
        public IList<AnswerBlockModel> Answers { get; set; }

        /// <summary>
        /// Получает или задает коллекцию ответов пользователя.
        /// </summary>
        public IList<string> UserAnswer { get; set; }
    }
}
