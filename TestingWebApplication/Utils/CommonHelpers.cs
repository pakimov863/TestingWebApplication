namespace TestingWebApplication.Utils
{
    using System;
    using System.Linq;
    using Data.Repository.Model;

    public static class CommonHelpers
    {
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
    }
}
