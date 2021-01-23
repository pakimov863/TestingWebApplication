namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.ComponentModel.DataAnnotations;
    using Data.Shared;

    public class AnswerBlockViewModel
    {
        public long BlockId { get; set; }

        [Required]
        public string Text { get; set; }

        
        [Required]
        public bool IsCorrect { get; set; }

        
        [Required]
        public  AnswerBlockType AnswerType { get; set; }
    }
}
