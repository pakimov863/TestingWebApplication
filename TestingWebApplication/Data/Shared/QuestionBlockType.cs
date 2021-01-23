namespace TestingWebApplication.Data.Shared
{
    /// <summary>
    /// Тип блока-вопроса.
    /// </summary>
    public enum QuestionBlockType
    {
        /// <summary>
        /// Выводить как текст.
        /// </summary>
        Text,
        
        /// <summary>
        /// Выводить как выражение LaTeX.
        /// </summary>
        Latex,

        /// <summary>
        /// Выводить как ссылку на картинку.
        /// </summary>
        Image
    }
}
