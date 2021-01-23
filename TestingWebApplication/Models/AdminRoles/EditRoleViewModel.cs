namespace TestingWebApplication.Models.AdminRoles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о редактируемой роли.
    /// </summary>
    public class EditRoleViewModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EditRoleViewModel"/>.
        /// </summary>
        public EditRoleViewModel()
        {
            UsersInRole = new List<UserRoleViewModel>();
        }

        /// <summary>
        /// Получает или задает идентификатор роли.
        /// </summary>
        [Required]
        [Display(Name = "Идентификатор роли")]
        public string RoleId { get; set; }

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
