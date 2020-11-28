namespace TestingWebApplication.Data.Database
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Shared;

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
                            new AnswerBlockDto {Text = "Text of answer 1-1!", AnswerType = AnswerBlockType.Radio,  IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 1-2", AnswerType = AnswerBlockType.Radio}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 2?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 2-1", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 2-2!", AnswerType = AnswerBlockType.Radio, IsCorrect = true}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 3?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 3-1!", AnswerType = AnswerBlockType.Checkbox, IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 3-2!", AnswerType = AnswerBlockType.Checkbox, IsCorrect = true}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 4?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 4-1", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 4-2!", AnswerType = AnswerBlockType.Radio, IsCorrect = true}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 5?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "CORRECT", AnswerType = AnswerBlockType.Text, IsCorrect = true}
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
                            new AnswerBlockDto {Text = "Text of answer 1-1!", AnswerType = AnswerBlockType.Radio, IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 1-2", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 1-3", AnswerType = AnswerBlockType.Radio}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 2?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 2-1!", AnswerType = AnswerBlockType.Checkbox, IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 2-2!", AnswerType = AnswerBlockType.Checkbox, IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 2-3", AnswerType = AnswerBlockType.Checkbox}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 3?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 3-1", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 3-2", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 3-3!", AnswerType = AnswerBlockType.Radio, IsCorrect = true}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 4?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 4-1", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 4-2", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 4-3", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 4-4!", AnswerType = AnswerBlockType.Radio, IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 4-5", AnswerType = AnswerBlockType.Radio}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 5?"},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "CORRECT", AnswerType = AnswerBlockType.Text, IsCorrect = true}
                        }
                    }
                },
                Creator = userFromDb
            };

            context.Quizzes.Add(quiz1);
            context.Quizzes.Add(quiz2);
            context.SaveChanges();
        }

        public static void Clear(AppDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}
