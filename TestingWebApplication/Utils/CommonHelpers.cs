namespace TestingWebApplication.Utils
{
    using System;
    using System.Linq;
    using Data.Database.Model;
    using Data.Repository.Model;
    using Data.Shared;

    /// <summary>
    /// Утилитарный класс с общими вспомогательными методами.
    /// </summary>
    public static class CommonHelpers
    {
        /// <summary>
        /// Выполняет перемешивание вопросов и ответов в тесте.
        /// </summary>
        /// <param name="quiz">Исходный тест.</param>
        /// <returns>Тест с перемешанными данными.</returns>
        public static GeneratedQuizModel ShuffleQuizData(GeneratedQuizModel quiz)
        {
            var rnd = new Random(quiz.StartTime.Millisecond);
            quiz.SourceQuiz.QuizBlocks = quiz.SourceQuiz.QuizBlocks.Shuffle(rnd).ToList();
            foreach (var quizBlock in quiz.SourceQuiz.QuizBlocks)
            {
                quizBlock.Answers = quizBlock.Answers.Shuffle(rnd).ToList();
            }

            return quiz;
        }

        /// <summary>
        /// Выполняет проверку ответов пользователя
        /// </summary>
        /// <param name="sourceQuizBlock">Исходный блок теста.</param>
        /// <param name="answeredQuizBlock">Блок теста с ответом пользователя.</param>
        /// <returns>Значение, показывающее, правильно ли ответил пользователь.</returns>
        public static bool CheckQuizAnswer(QuizBlockDto sourceQuizBlock, UserAnswerDto answeredQuizBlock)
        {
            var userAnswerList = answeredQuizBlock.UserAnswer.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            var blockAnswerList = sourceQuizBlock.Answers.Where(e => e.IsCorrect).ToList();

            //// Если нет пользовательских ответов - ответ некорректный.
            if (!userAnswerList.Any())
            {
                return false;
            }

            //// Проверим ответы.
            var allIsCorrect = true;

            foreach (var blockAnswer in blockAnswerList)
            {
                switch (blockAnswer.AnswerType)
                {
                    case AnswerBlockType.Text:
                        var firstUserAnswer = userAnswerList.First().Trim();
                        var firstBlockAnswer = blockAnswer.Text.Trim();
                        allIsCorrect &= firstBlockAnswer.Equals(firstUserAnswer, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case AnswerBlockType.Checkbox:
                    case AnswerBlockType.Radio:
                        allIsCorrect &= userAnswerList.Contains(blockAnswer.Id.ToString());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(blockAnswer.AnswerType));
                }
            }

            return allIsCorrect;
        }
    }
}
