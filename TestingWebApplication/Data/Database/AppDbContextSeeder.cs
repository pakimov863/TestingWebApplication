namespace TestingWebApplication.Data.Database
{
    using System.Collections.Generic;
    using Model;
    using Shared;

    /// <summary>
    /// Утилитарный класс, содержащий вспомогательные действия для работы с базой данных.
    /// </summary>
    public static class AppDbContextSeeder
    {
        /// <summary>
        /// Выполняет заполнение базы данных тестовыми данными.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public static void Seed(AppDbContext context)
        {
            //// Группы.
            var group1 = new UserGroupDto {Title = "Преподаватели", Visible = false};
            context.UserGroups.Add(group1);
            var group2 = new UserGroupDto {Title = "1А", Visible = true};
            context.UserGroups.Add(group2);
            context.SaveChanges();

            //// Пользователи.
            var user1 = new UserDto {UserName = "Admin", Password = "1"};
            context.Users.Add(user1);
            var user2 = new UserDto {UserName = "Иван Петров", Password = string.Empty};
            context.Users.Add(user2);
            var user3 = new UserDto {UserName = "Алексей Иванов", Password = string.Empty};
            context.Users.Add(user3);
            var user4 = new UserDto {UserName = "Сергей Алексеев", Password = string.Empty};
            context.Users.Add(user4);
            context.SaveChanges();

            //// Привязки к группам.
            var groupLinker1 = new UserGroupLinkerDto {LinkedUser = user1, LinkedGroup = group1};
            context.UserGroupLinkers.Add(groupLinker1);
            var groupLinker2 = new UserGroupLinkerDto {LinkedUser = user2, LinkedGroup = group2};
            context.UserGroupLinkers.Add(groupLinker2);
            var groupLinker3 = new UserGroupLinkerDto {LinkedUser = user3, LinkedGroup = group2};
            context.UserGroupLinkers.Add(groupLinker3);
            var groupLinker4 = new UserGroupLinkerDto {LinkedUser = user4, LinkedGroup = group2};
            context.UserGroupLinkers.Add(groupLinker4);
            context.SaveChanges();

            //// Тесты.
            var quiz1 = new QuizDto
            {
                Title = "Title of quiz 1",
                TotalTimeSecs = 5000,
                QuizBlocks = new List<QuizBlockDto>
                {
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 1?", QuestionType = QuestionBlockType.Text},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 1-1!", AnswerType = AnswerBlockType.Radio,  IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 1-2", AnswerType = AnswerBlockType.Radio}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 2?", QuestionType = QuestionBlockType.Text},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 2-1", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 2-2!", AnswerType = AnswerBlockType.Radio, IsCorrect = true}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 3?", QuestionType = QuestionBlockType.Text},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 3-1!", AnswerType = AnswerBlockType.Checkbox, IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 3-2!", AnswerType = AnswerBlockType.Checkbox, IsCorrect = true}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 4?", QuestionType = QuestionBlockType.Text},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 4-1", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 4-2!", AnswerType = AnswerBlockType.Radio, IsCorrect = true}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 5?", QuestionType = QuestionBlockType.Text},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "CORRECT", AnswerType = AnswerBlockType.Text, IsCorrect = true}
                        }
                    }
                },
                Creator = user1
            };

            var quiz2 = new QuizDto
            {
                Title = "Title of quiz 2",
                TotalTimeSecs = 2000,
                QuizBlocks = new List<QuizBlockDto>
                {
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 1?", QuestionType = QuestionBlockType.Text},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 1-1!", AnswerType = AnswerBlockType.Radio, IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 1-2", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 1-3", AnswerType = AnswerBlockType.Radio}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 2?", QuestionType = QuestionBlockType.Text},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 2-1!", AnswerType = AnswerBlockType.Checkbox, IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 2-2!", AnswerType = AnswerBlockType.Checkbox, IsCorrect = true},
                            new AnswerBlockDto {Text = "Text of answer 2-3", AnswerType = AnswerBlockType.Checkbox}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 3?", QuestionType = QuestionBlockType.Text},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "Text of answer 3-1", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 3-2", AnswerType = AnswerBlockType.Radio},
                            new AnswerBlockDto {Text = "Text of answer 3-3!", AnswerType = AnswerBlockType.Radio, IsCorrect = true}
                        }
                    },
                    new QuizBlockDto
                    {
                        Question = new QuestionBlockDto {Text = "Text of question 4?", QuestionType = QuestionBlockType.Text},
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
                        Question = new QuestionBlockDto {Text = "Text of question 5?", QuestionType = QuestionBlockType.Text},
                        Answers = new List<AnswerBlockDto>
                        {
                            new AnswerBlockDto {Text = "CORRECT", AnswerType = AnswerBlockType.Text, IsCorrect = true}
                        }
                    }
                },
                Creator = user1
            };

            context.Quizzes.Add(quiz1);
            context.Quizzes.Add(quiz2);
            context.SaveChanges();
        }

        /// <summary>
        /// Выполняет очистку базы данных.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public static void Clear(AppDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}
