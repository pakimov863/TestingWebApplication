namespace TestingWebApplication.Data.Database.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Описание хранимой связи между пользователем и группой
    /// </summary>
    public class UserGroupLinkerDto
    {
        /// <summary>
        /// Получает или задает идентификатор связи.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает связанного пользователя.
        /// </summary>
        public virtual UserDto LinkedUser { get; set; }

        /// <summary>
        /// Получает или задает связанную группу.
        /// </summary>
        public virtual UserGroupDto LinkedGroup { get; set; }
    }
}
