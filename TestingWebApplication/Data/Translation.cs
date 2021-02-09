namespace TestingWebApplication.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Database.Model;
    using Repository.Model;

    /// <summary>
    /// Утилитарный класс с методами преобразования данных.
    /// </summary>
    public static class Translation
    {
        /// <summary>
        /// Выполняет преобразование <see cref="QuestionBlockDto"/> в <see cref="QuestionBlockModel"/>.
        /// </summary>
        /// <param name="dto">Исходный объект.</param>
        /// <returns>Преобразованный объект.</returns>
        public static QuestionBlockModel Translate(QuestionBlockDto dto)
        {
            var model = new QuestionBlockModel
            {
                Text = dto.Text,
            };

            return model;
        }

        /// <summary>
        /// Выполняет преобразование <see cref="AnswerBlockDto"/> в <see cref="AnswerBlockModel"/>.
        /// </summary>
        /// <param name="dto">Исходный объект.</param>
        /// <returns>Преобразованный объект.</returns>
        public static AnswerBlockModel Translate(AnswerBlockDto dto)
        {
            var model = new AnswerBlockModel
            {
                Id = dto.Id,
                Text = dto.Text,
                AnswerType = dto.AnswerType,
                IsCorrect = dto.IsCorrect,
            };

            return model;
        }

        /// <summary>
        /// Выполняет преобразование <see cref="QuizBlockDto"/> в <see cref="QuizBlockModel"/>.
        /// </summary>
        /// <param name="dto">Исходный объект.</param>
        /// <returns>Преобразованный объект.</returns>
        public static QuizBlockModel Translate(QuizBlockDto dto)
        {
            var model = new QuizBlockModel
            {
                Id = dto.Id,
                Question = Translate(dto.Question),
                Answers = dto.Answers.Select(Translate).ToList(),
                UserAnswer = new List<string>(),
            };

            return model;
        }

        /// <summary>
        /// Выполняет преобразование <see cref="QuizDto"/> в <see cref="QuizModel"/>.
        /// </summary>
        /// <param name="dto">Исходный объект.</param>
        /// <returns>Преобразованный объект.</returns>
        public static QuizModel Translate(QuizDto dto)
        {
            var model = new QuizModel
            {
                Id = dto.Id,
                Title = dto.Title,
                TotalTimeSecs = dto.TotalTimeSecs,
                MaxQuizBlocks = dto.MaxQuizBlocksCount,
                QuizBlocks = dto.QuizBlocks.Select(Translate).ToList(),
            };

            return model;
        }

        /// <summary>
        /// Выполняет преобразование <see cref="GeneratedQuizDto"/> в <see cref="GeneratedQuizModel"/>.
        /// </summary>
        /// <param name="dto">Исходный объект.</param>
        /// <returns>Преобразованный объект.</returns>
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

        /// <summary>
        /// Выполняет преобразование <see cref="UserDto"/> в <see cref="SimpleUserModel"/>.
        /// </summary>
        /// <param name="dto">Исходный объект.</param>
        /// <returns>Преобразованный объект.</returns>
        public static SimpleUserModel SimpleTranslate(UserDto dto)
        {
            var model = new SimpleUserModel
            {
                Id = dto.Id,
                UserName = dto.UserName,
            };

            return model;
        }

        /// <summary>
        /// Выполняет преобразование <see cref="QuizDto"/> в <see cref="SimpleQuizModel"/>.
        /// </summary>
        /// <param name="dto">Исходный объект.</param>
        /// <returns>Преобразованный объект.</returns>
        public static SimpleQuizModel SimpleTranslate(QuizDto dto)
        {
            var model = new SimpleQuizModel
            {
                Id = dto.Id,
                Title = dto.Title,
                TotalTimeSecs = dto.TotalTimeSecs,
            };

            return model;
        }
    }
}