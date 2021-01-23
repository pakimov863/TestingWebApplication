namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditQuizViewModel
    {
        public EditQuizViewModel()
        {
            QuizBlocks = new List<QuizBlockViewModel>();
        }

        [Required]
        public long QuizId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public long TotalTimeSecs { get; set; }

        [Required]
        public List<QuizBlockViewModel> QuizBlocks { get; set; }
    }
}
