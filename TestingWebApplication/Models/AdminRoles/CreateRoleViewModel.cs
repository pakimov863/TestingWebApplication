namespace TestingWebApplication.Models.AdminRoles
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о создаваемой роли.
    /// </summary>
    public class CreateRoleViewModel
    {
        /// <summary>
        /// Получает или задает название роли.
        /// </summary>
        [Required]
        [Display(Name = "Название роли")]
        public string RoleName { get; set; }
    }
}
