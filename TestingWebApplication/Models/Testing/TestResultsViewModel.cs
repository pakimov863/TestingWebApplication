namespace TestingWebApplication.Models.Testing
{
    using System;

    public class TestResultsViewModel
    {
        public string TestTitle { get; set; }

        public DateTime StartTime { get; set; }

        public int QuestionCount { get; set; }

        public int CorrectAnswersCount { get; set; }

        public int CorrectAnswersPercent => CorrectAnswersCount * 100 / QuestionCount;
    }
}
