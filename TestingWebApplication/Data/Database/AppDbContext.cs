namespace TestingWebApplication.Data.Database
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Model;

    /// <summary>
    /// Класс, описывающий контекст базы данных.
    /// </summary>
    public class AppDbContext : IdentityDbContext<UserDto>
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

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }
    }
}
