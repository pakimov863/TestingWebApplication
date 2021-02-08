namespace TestingWebApplication.Models.AdminRoles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о создаваемой роли.
    /// </summary>
    public class CreateRoleViewModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CreateRoleViewModel"/>.
        /// </summary>
        public CreateRoleViewModel()
        {
            UsersInRole = new List<UserRoleViewModel>();
        }

        /// <summary>
        /// Получает или задает название роли.
        /// </summary>
        [Required]
        [Display(Name = "Название роли")]
        public string RoleName { get; set; }

        /// <summary>
        /// Получает или задает коллекцию пользователей.
        /// </summary>
        public List<UserRoleViewModel> UsersInRole { get; set; }
    }
}
