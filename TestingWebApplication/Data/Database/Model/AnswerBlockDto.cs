namespace TestingWebApplication.Data.Database.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Shared;

    public class AnswerBlockDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        public AnswerBlockType AnswerType { get; set; }

        public virtual QuizBlockDto Quiz { get; set; }
    }
}
