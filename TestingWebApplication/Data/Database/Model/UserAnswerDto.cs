namespace TestingWebApplication.Data.Database.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserAnswerDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long GeneratedQuizId { get; set; }

        public long QuizBlockId { get; set; }

        public string UserAnswer { get; set; }

        public virtual GeneratedQuizDto LinkedQuiz { get; set; }
    }
}
