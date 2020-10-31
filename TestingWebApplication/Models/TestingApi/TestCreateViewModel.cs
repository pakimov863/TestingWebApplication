namespace TestingWebApplication.Models.TestingApi
{
    using System.ComponentModel.DataAnnotations;

    public class TestCreateViewModel
    {
        [Required]
        public long QuizId { get; set; }
        
        [Required]
        public string Username { get; set; }
    }
}
