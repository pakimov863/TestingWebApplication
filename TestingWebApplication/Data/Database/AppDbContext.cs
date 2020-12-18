namespace TestingWebApplication.Data.Database
{
    using Microsoft.EntityFrameworkCore;
    using Model;

    /// <summary>
    /// Класс, описывающий контекст базы данных.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляра класса <see cref="AppDbContext"/>.
        /// </summary>
        /// <param name="options">Параметры настройки подключения к БД.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Получает или задает таблицу с пользователями.
        /// </summary>
        public DbSet<UserDto> Users { get; set; }
        
        /// <summary>
        /// Получает или задает таблицу с тестами.
        /// </summary>
        public DbSet<QuizDto> Quizzes { get; set; }
        
        /// <summary>
        /// Получает или задает таблицу с блоками теста.
        /// </summary>
        public DbSet<QuizBlockDto> QuizBlocks { get; set; }

        /// <summary>
        /// Получает или задает таблицу с блоками вопросов.
        /// </summary>
        public DbSet<QuestionBlockDto> Questions { get; set; }

        /// <summary>
        /// Получает или задает таблицу с блоками ответов.
        /// </summary>
        public DbSet<AnswerBlockDto> Answers { get; set; }

        /// <summary>
        /// Получает или задает таблицу с пользовательскими ответами.
        /// </summary>
        public DbSet<UserAnswerDto> UserAnswers { get; set; }

        /// <summary>
        /// Получает или задает таблицу с сгенерированными тестами.
        /// </summary>
        public DbSet<GeneratedQuizDto> UserQuizzes { get; set; }

        /// <summary>
        /// Получает или задает таблицу с группами пользователей.
        /// </summary>
        public DbSet<UserGroupDto> UserGroups { get; set; }

        /// <summary>
        /// Получает или задает таблицу с связями между пользователями и группами.
        /// </summary>
        public DbSet<UserGroupLinkerDto> UserGroupLinkers { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            InitializePrimaryKeys(modelBuilder);
            InitializeLinks(modelBuilder);
        }

        /// <summary>
        /// Выполняет настройку первичных ключей для таблиц.
        /// </summary>
        /// <param name="modelBuilder">Строитель модели БД.</param>
        private void InitializePrimaryKeys(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserDto>()
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<UserDto>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<QuizDto>()
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<QuizDto>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<QuizBlockDto>()
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<QuizBlockDto>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<QuestionBlockDto>()
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<QuestionBlockDto>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<AnswerBlockDto>()
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<AnswerBlockDto>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<UserAnswerDto>()
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<UserAnswerDto>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<GeneratedQuizDto>()
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<GeneratedQuizDto>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<UserGroupDto>()
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<UserGroupDto>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<UserGroupLinkerDto>()
                .HasKey(e => e.Id);
            modelBuilder
                .Entity<UserGroupLinkerDto>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
        
        /// <summary>
        /// Выполняет настройку связей между таблицами.
        /// </summary>
        /// <param name="modelBuilder">Строитель модели БД.</param>
        private void InitializeLinks(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<AnswerBlockDto>()
                .Property(e => e.AnswerType)
                .HasConversion<string>();
            modelBuilder
                .Entity<QuestionBlockDto>()
                .Property(e => e.QuestionType)
                .HasConversion<string>();
            modelBuilder
                .Entity<QuizBlockDto>()
                .HasOne(e => e.Question)
                .WithOne(e => e.Quiz)
                .HasForeignKey<QuestionBlockDto>(e => e.QuizId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<QuizBlockDto>()
                .HasMany(e => e.Answers)
                .WithOne(e => e.Quiz)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<QuizDto>()
                .HasMany(e => e.QuizBlocks)
                .WithOne(e => e.Quiz)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<UserDto>()
                .HasMany(e => e.CreatedQuizzes)
                .WithOne(e => e.Creator)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<UserDto>()
                .HasMany(e => e.RespondedQuizzes)
                .WithOne(e => e.RespondentUser)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<GeneratedQuizDto>()
                .HasMany(e => e.UserAnswers)
                .WithOne(e => e.LinkedQuiz)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<UserDto>()
                .HasMany(e => e.GroupLinks)
                .WithOne(e => e.LinkedUser)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<UserGroupDto>()
                .HasMany(e => e.UserLinks)
                .WithOne(e => e.LinkedGroup)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
