namespace TestingWebApplication.Data.Repository.Model
{
    using System;

    public class GeneratedQuizModel
    {
        public long Id { get; set; }

        public DateTime StartTime { get; set; }

        public QuizModel SourceQuiz { get; set; }
    }
}
