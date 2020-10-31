namespace TestingWebApplication.Data.Database
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public static class AppDbContextSeeder
    {
        public static void Seed(AppDbContext context)
        {
            var user1 = new UserDto { UserName = "Admin", Password = "1" };
            context.Users.Add(user1);
            context.SaveChanges();
            var userFromDb = context.Users.First();

            var quiz1 = new QuizDto
            {
                Title = "Title of quiz 1",
                TotalTimeSecs = 5000,
                QuizBlocks = new List<QuizBlockDto>
                {
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 1?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 1-1", IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 1-2"}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 2?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 2-1"},
                            new AnswerBlockDto {Text = "Text of answer 2-2", IsCorrect = true}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 3?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 3-1", IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 3-2"}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 4?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 4-1"},
                            new AnswerBlockDto {Text = "Text of answer 4-2", IsCorrect = true}
                        }
                    }
                },
                Creator = userFromDb
            };

            var quiz2 = new QuizDto
            {
                Title = "Title of quiz 2",
                TotalTimeSecs = 2000,
                QuizBlocks = new List<QuizBlockDto>
                {
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 1?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 1-1", IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 1-2"},
                            new AnswerBlockDto {Text = "Text of answer 1-3"}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 2?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 2-1"},
                            new AnswerBlockDto {Text = "Text of answer 2-2", IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 2-3"}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 3?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 3-1"},
                            new AnswerBlockDto {Text = "Text of answer 3-2"},
                            new AnswerBlockDto {Text = "Text of answer 3-3", IsCorrect = true}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 4?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 4-1"},
                            new AnswerBlockDto {Text = "Text of answer 4-2"},
                            new AnswerBlockDto {Text = "Text of answer 4-3"},
                            new AnswerBlockDto {Text = "Text of answer 4-4", IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 4-5"}
                        }
                    }
                },
                Creator = userFromDb
            };

            context.Quizzes.Add(quiz1);
            context.Quizzes.Add(quiz2);
            context.SaveChanges();

            var user = context.Users.First();

            var i = 1;
        }

        public static void Clear(AppDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}
