namespace TestingWebApplication.Data.Database.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class QuizBlockDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public virtual QuizDto Quiz { get; set; }

        public virtual QuestionBlockDto Question { get; set; }

        public ICollection<AnswerBlockDto> Answers { get; set; }
    }
}
