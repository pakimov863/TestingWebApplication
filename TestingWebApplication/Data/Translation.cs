namespace TestingWebApplication.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Database.Model;
    using Repository.Model;

    public static class Translation
    {
        public static QuestionBlockModel Translate(QuestionBlockDto dto)
        {
            var model = new QuestionBlockModel
            {
                Text = dto.Text,
            };

            return model;
        }

        public static AnswerBlockModel Translate(AnswerBlockDto dto)
        {
            var model = new AnswerBlockModel
            {
                Id = dto.Id,
                Text = dto.Text,
                IsCorrect = dto.IsCorrect,
            };

            return model;
        }

        public static QuizBlockModel Translate(QuizBlockDto dto)
        {
            var model = new QuizBlockModel
            {
                Id = dto.Id,
                Question = Translate(dto.Question),
                Answers = dto.Answers.Select(Translate).ToList(),
                UserAnswer = new List<string>(),
            };

            if (dto.Answers.Count == 1)
            {
                model.AnswersType = AnswerViewType.Text;
            }
            else if (dto.Answers.Count(e => e.IsCorrect) == 1)
            {
                model.AnswersType = AnswerViewType.Radio;
            }
            else if (dto.Answers.Count(e => e.IsCorrect) > 1)
            {
                model.AnswersType = AnswerViewType.Checkbox;
            }

            return model;
        }

        public static QuizModel Translate(QuizDto dto)
        {
            var model = new QuizModel
            {
                Title = dto.Title,
                TotalTimeSecs = dto.TotalTimeSecs,
                QuizBlocks = dto.QuizBlocks.Select(Translate).ToList(),
            };

            return model;
        }

        public static GeneratedQuizModel Translate(GeneratedQuizDto dto)
        {
            var model = new GeneratedQuizModel
            {
                Id = dto.Id,
                StartTime = dto.StartTime,
                SourceQuiz = Translate(dto.SourceQuiz),
            };

            return model;
        }
    }
}