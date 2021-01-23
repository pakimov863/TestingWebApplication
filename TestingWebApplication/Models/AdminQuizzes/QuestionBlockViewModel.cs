namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.ComponentModel.DataAnnotations;
    using Data.Shared;

    public class QuestionBlockViewModel
    {
        public long BlockId { get; set; }

        [Required]
        public string Text { get; set; }
        
        [Required]
        public QuestionBlockType QuestionType { get; set; }
    }
}
