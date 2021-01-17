using System.ComponentModel.DataAnnotations;

namespace TestingWebApplication.Models.AdminRoles
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
