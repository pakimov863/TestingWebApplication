namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateQuizViewModel
    {
        public CreateQuizViewModel()
        {
            QuizBlocks = new List<QuizBlockViewModel>();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public long TotalTimeSecs { get; set; }

        public List<QuizBlockViewModel> QuizBlocks { get; set; }
    }
}
