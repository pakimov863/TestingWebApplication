namespace TestingWebApplication.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о входе пользователя.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Получает или задает логин для входа.
        /// </summary>
        [Required]
        [Display(Name = "Логин пользователя")]
        public string Login { get; set; }

        /// <summary>
        /// Получает или задает пароль для входа.
        /// </summary>
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
