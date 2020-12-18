namespace TestingWebApplication.Data.Database.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Описание хранимой группы пользователей.
    /// </summary>
    public class UserGroupDto
    {
        /// <summary>
        /// Получает или задает идентификатор группы.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает название группы.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Получает или задает значение, показывающее, является ли группа видимой.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Получает или задает коллекцию связей к пользователям, которые принадлежат группе.
        /// </summary>
        public virtual IList<UserGroupLinkerDto> UserLinks { get; set; }
    }
}
