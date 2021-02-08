namespace TestingWebApplication.Models.AdminRoles
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о привязанных к роли пользователях.
    /// </summary>
    public class UserRoleViewModel
    {
        /// <summary>
        /// Получает или задает идентификатор пользователя.
        /// </summary>
        [Required]
        [Display(Name = "Идентификатор пользователя")]
        public string UserId { get; set; }

        /// <summary>
        /// Получает или задает логин пользователя.
        /// </summary>
        [Required]
        [Display(Name = "Логин пользователя")]
        public string UserName { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя.
        /// </summary>
        [Required]
        [Display(Name = "Имя пользователя")]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает значение, показывающее, выбран ли пользователь.
        /// </summary>
        [Required]
        [Display(Name = "Привязан к роли")]
        public bool IsSelected { get; set; }
    }
}
