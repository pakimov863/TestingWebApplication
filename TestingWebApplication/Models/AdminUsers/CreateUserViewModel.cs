namespace TestingWebApplication.Models.AdminUsers
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о создаваемом пользователе.
    /// </summary>
    public class CreateUserViewModel
    {
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
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль пользователя")]
        public string Password { get; set; }

        /// <summary>
        /// Получает или задает подтверждение пароля.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }
}
