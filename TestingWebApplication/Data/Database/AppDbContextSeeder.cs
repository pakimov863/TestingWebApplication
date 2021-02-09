namespace TestingWebApplication.Data.Database
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using Model;
    using Shared;

    /// <summary>
    /// Утилитарный класс, содержащий вспомогательные действия для работы с базой данных.
    /// </summary>
    public static class AppDbContextSeeder
    {
        /// <summary>
        /// Выполняет заполнение базы данных.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        /// <param name="userManager">Менеджер пользователей.</param>
        /// <param name="roleManager">Менеджер ролей.</param>
        public static void SeedProduction(AppDbContext context, UserManager<UserDto> userManager, RoleManager<IdentityRole> roleManager)
        {
            //// Пользователи.
            var user1 = InitializeUser(userManager, "Administrator", "administrator", "1111111111");

            //// Роли.
            var role1 = InitializeRole(roleManager, "Admin");
            var role2 = InitializeRole(roleManager, "User");

            //// Связывание ролей и пользователей.
            ConnectUserToRole(userManager, user1, role1.Name);

            context.SaveChanges();
        }

        /// <summary>
        /// Выполняет заполнение базы данных тестовыми данными.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        /// <param name="userManager">Менеджер пользователей.</param>
        /// <param name="roleManager">Менеджер ролей.</param>
        public static void SeedTesting(AppDbContext context, UserManager<UserDto> userManager, RoleManager<IdentityRole> roleManager)
        {
            //// Пользователи.
            var user1 = InitializeUser(userManager, "Administrator", "administrator", "1111111111");
            var user2 = InitializeUser(userManager, "Иван Петров", "i.petrov", "1111111111");
            var user3 = InitializeUser(userManager, "Алексей Иванов", "a.ivanov", "1111111111");
            var user4 = InitializeUser(userManager, "Сергей Алексеев", "s.alexeev", "1111111111");

            //// Роли.
            var role1 = InitializeRole(roleManager, "Admin");
            var role2 = InitializeRole(roleManager, "User");

            //// Связывание ролей и пользователей.
            ConnectUserToRole(userManager, user1, role1.Name);
            ConnectUserToRole(userManager, user2, role2.Name);
            ConnectUserToRole(userManager, user3, role2.Name);
            ConnectUserToRole(userManager, user4, role2.Name);

            //// Тесты.
            var quiz1 = new QuizDto
            {
                Title = "Title of quiz 1",
                TotalTimeSecs = 5000,
                MaxQuizBlocksCount = 5,
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
                MaxQuizBlocksCount = 3,
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

        /// <summary>
        /// Создает и сохраняет пользователя с заданными параметрами.
        /// </summary>
        /// <param name="userManager">Менеджер пользователей.</param>
        /// <param name="name">Имя пользователя.</param>
        /// <param name="userName">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Созданный пользователь.</returns>
        private static UserDto InitializeUser(UserManager<UserDto> userManager, string name, string userName, string password)
        {
            var user = new UserDto
            {
                Name = name,
                UserName = userName,
                CreatedQuizzes = new List<QuizDto>(),
                RespondedQuizzes = new List<GeneratedQuizDto>()
            };

            var result = userManager.CreateAsync(user, password).GetAwaiter().GetResult();
            if (!result.Succeeded)
            {
                throw new Exception($"Cannot create user: {userName}.");
            }

            return user;
        }

        /// <summary>
        /// Создает и сохраняет роль с заданными параметрами.
        /// </summary>
        /// <param name="roleManager">Менеджер ролей.</param>
        /// <param name="name">Название роли.</param>
        /// <returns>Созданная роль.</returns>
        private static IdentityRole InitializeRole(RoleManager<IdentityRole> roleManager, string name)
        {
            var role = new IdentityRole
            {
                Name = name
            };

            var result = roleManager.CreateAsync(role).GetAwaiter().GetResult();
            if (!result.Succeeded)
            {
                throw new Exception($"Cannot create role: {name}.");
            }

            return role;
        }

        /// <summary>
        /// Задает роль пользователю.
        /// </summary>
        /// <param name="userManager">Менеджер пользователей.</param>
        /// <param name="user">Существующий пользователь.</param>
        /// <param name="roleName">Название роли.</param>
        private static void ConnectUserToRole(UserManager<UserDto> userManager, UserDto user, string roleName)
        {
            var needConnect = !userManager.IsInRoleAsync(user, roleName).GetAwaiter().GetResult();
            if (!needConnect)
            {
                return;
            }

            userManager.AddToRoleAsync(user, roleName).GetAwaiter().GetResult();
        }
    }
}
