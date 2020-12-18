namespace TestingWebApplication.Data.Database.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Описание хранимого пользователя.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Получает или задает идентификатор пользователя.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Получает или задает пароль пользователя.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Получает или задает коллекцию тестов, созданных пользователем.
        /// </summary>
        public virtual IList<QuizDto> CreatedQuizzes { get; set; }

        /// <summary>
        /// Получает или задает коллекцию связей к группам, к которым принадлежит пользователь.
        /// </summary>
        public virtual IList<UserGroupLinkerDto> GroupLinks { get; set; }

        /// <summary>
        /// Получает или задает коллекцию пройденных пользователем тестов.
        /// </summary>
        public virtual IList<GeneratedQuizDto> RespondedQuizzes { get; set; }
    }
}
