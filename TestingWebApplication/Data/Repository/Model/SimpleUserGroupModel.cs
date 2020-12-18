namespace TestingWebApplication.Data.Repository.Model
{
    /// <summary>
    /// Упрощенная модель группы пользователя.
    /// </summary>
    public class SimpleUserGroupModel
    {
        /// <summary>
        /// Получает или задает идентификатор группы.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает название группы.
        /// </summary>
        public string Title { get; set; }
    }
}
