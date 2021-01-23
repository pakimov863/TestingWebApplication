namespace TestingWebApplication.Models.AdminUsers
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о редактируемом пользователе.
    /// </summary>
    public class EditUserViewModel
    {
        /// <summary>
        /// Получает или задает идентификатор пользователя.
        /// </summary>
        [Required]
        [Display(Name = "Идентификатор пользователя")]
        public string UserId { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя.
        /// </summary>
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        /// <summary>
        /// Получает или задает логин пользователя.
        /// </summary>
        [Required]
        [Display(Name = "Логин пользователя")]
        public string UserLogin { get; set; }

        /// <summary>
        /// Получает или задает пароль пользователя.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Пароль пользователя")]
        public string Password { get; set; }

        /// <summary>
        /// Получает или задает подтверждение пароля.
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }
}
