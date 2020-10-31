namespace TestingWebApplication.Data.Database.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class QuizDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Title { get; set; }

        public long TotalTimeSecs { get; set; }

        public virtual UserDto Creator { get; set; }

        public ICollection<QuizBlockDto> QuizBlocks { get; set; }
    }
}
