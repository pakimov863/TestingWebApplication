namespace TestingWebApplication.Data.Database.Model
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    /// <summary>
    /// Описание хранимого пользователя.
    /// </summary>
    public class UserDto : IdentityUser
    {
        /// <summary>
        /// Получает или задает отображаемое имя пользователя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает коллекцию тестов, созданных пользователем.
        /// </summary>
        public virtual IList<QuizDto> CreatedQuizzes { get; set; }

        /// <summary>
        /// Получает или задает коллекцию пройденных пользователем тестов.
        /// </summary>
        public virtual IList<GeneratedQuizDto> RespondedQuizzes { get; set; }
    }
}
