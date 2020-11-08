namespace TestingWebApplication.Data.Database.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class GeneratedQuizDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Tag { get; set; }

        public bool IsEnded { get; set; }

        public DateTime StartTime { get; set; }

        public virtual QuizDto SourceQuiz { get; set; }

        public virtual UserDto RespondentUser { get; set; }

        public virtual ICollection<UserAnswerDto> UserAnswers { get; set; }
    }
}
