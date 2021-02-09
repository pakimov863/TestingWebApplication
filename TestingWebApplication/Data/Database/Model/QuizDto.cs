namespace TestingWebApplication.Data.Database.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Описание хранимого теста.
    /// </summary>
    public class QuizDto
    {
        /// <summary>
        /// Получает или задает идентификатор теста.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает заголовок теста.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Получает или задает время на прохождение теста.
        /// </summary>
        public long TotalTimeSecs { get; set; }

        /// <summary>
        /// Получает или задает максимальное количество тестовых блоков в сгенерированном тесте.
        /// </summary>
        public int MaxQuizBlocksCount { get; set; }

        /// <summary>
        /// Получает или задает пользователя, создавшего тест.
        /// </summary>
        public virtual UserDto Creator { get; set; }

        /// <summary>
        /// Получает или задает коллекцию блоков теста.
        /// </summary>
        public IList<QuizBlockDto> QuizBlocks { get; set; }
    }
}
