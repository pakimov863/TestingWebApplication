namespace TestingWebApplication.Data.Repository.Model
{
    /// <summary>
    /// Упрощенная модель пользователя.
    /// </summary>
    public class SimpleUserModel
    {
        /// <summary>
        /// Получает или задает идентификатор пользователя.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя.
        /// </summary>
        public string UserName { get; set; }
    }
}
