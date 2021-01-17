namespace TestingWebApplication.Models.AdminRoles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            UsersInRole = new List<UserRoleViewModel>();
        }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        public List<UserRoleViewModel> UsersInRole { get; set; }
    }
}
