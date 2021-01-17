using System.ComponentModel.DataAnnotations;

namespace TestingWebApplication.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
