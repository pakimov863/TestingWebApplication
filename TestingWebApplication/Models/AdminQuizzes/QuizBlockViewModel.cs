namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class QuizBlockViewModel
    {
        public QuizBlockViewModel()
        {
            Answers = new List<AnswerBlockViewModel>();
        }

        public long BlockId { get; set; }

        [Required]
        public QuestionBlockViewModel Question { get; set; }

        public List<AnswerBlockViewModel> Answers { get; set; }
    }
}
