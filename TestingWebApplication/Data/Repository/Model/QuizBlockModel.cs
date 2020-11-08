namespace TestingWebApplication.Data.Repository.Model
{
    using System.Collections.Generic;

    public class QuizBlockModel
    {
        public long Id { get; set; }

        public QuestionBlockModel Question { get; set; }

        public IList<AnswerBlockModel> Answers { get; set; }

        public AnswerViewType AnswersType { get; set; }

        public IList<string> UserAnswer { get; set; }
    }
}
