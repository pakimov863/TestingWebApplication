using System.ComponentModel.DataAnnotations;

namespace TestingWebApplication.Models.AdminUsers
{
    public class CreateUserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserLogin { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
