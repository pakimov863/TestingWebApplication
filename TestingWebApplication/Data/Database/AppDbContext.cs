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
        /// 
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserDto> Users { get; set; }
        
        public DbSet<QuizDto> Quizzes { get; set; }
        
        public DbSet<QuizBlockDto> QuizBlocks { get; set; }

        public DbSet<QuestionBlockDto> Questions { get; set; }

        public DbSet<AnswerBlockDto> Answers { get; set; }

        public DbSet<UserAnswerDto> UserAnswers { get; set; }

        public DbSet<GeneratedQuizDto> UserQuizzes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            InitializePrimaryKeys(modelBuilder);
            InitializeLinks(modelBuilder);
        }

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
        }
        
        private void InitializeLinks(ModelBuilder modelBuilder)
        {
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
                .HasMany(e => e.Quizzes)
                .WithOne(e => e.Creator)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
