namespace TestingWebApplication.Data.Repository.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class QuizBlockModel
    {
        public long Id { get; set; }

        public QuestionBlockModel Question { get; set; }

        public IList<AnswerBlockModel> Answers { get; set; }

        public QuizBlockViewType AnswerViewType
        {
            get
            {
                if (Answers.Count == 1)
                {
                    return QuizBlockViewType.Text;
                }

                if (Answers.Count(e => e.IsCorrect) == 1)
                {
                    return QuizBlockViewType.Radio;
                }

                if (Answers.Count(e => e.IsCorrect) > 1)
                {
                    return QuizBlockViewType.Checkbox;
                }

                throw new ArgumentOutOfRangeException(nameof(Answers));
            }
        }
    }
}
