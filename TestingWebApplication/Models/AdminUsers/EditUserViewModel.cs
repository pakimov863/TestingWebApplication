namespace TestingWebApplication.Models.AdminUsers
{
    using System.ComponentModel.DataAnnotations;

    public class EditUserViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserLogin { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
