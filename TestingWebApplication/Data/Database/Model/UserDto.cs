namespace TestingWebApplication.Data.Database.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public virtual ICollection<QuizDto> CreatedQuizzes { get; set; }

        public virtual ICollection<GeneratedQuizDto> RespondedQuizzes { get; set; }
    }
}
