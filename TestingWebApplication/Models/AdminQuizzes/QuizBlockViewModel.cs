namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о блоке теста.
    /// </summary>
    public class QuizBlockViewModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="QuizBlockViewModel"/>.
        /// </summary>
        public QuizBlockViewModel()
        {
            Answers = new List<AnswerBlockViewModel>();
        }

        /// <summary>
        /// Получает или задает идентификатор блока.
        /// </summary>
        [Display(Name = "Идентификатор блока")]
        public long BlockId { get; set; }

        /// <summary>
        /// Получает или задает блок вопроса.
        /// </summary>
        [Required]
        [Display(Name = "Описание вопроса")]
        public QuestionBlockViewModel Question { get; set; }

        /// <summary>
        /// Получает или задает коллекцию блоков ответов.
        /// </summary>
        public List<AnswerBlockViewModel> Answers { get; set; }
    }
}
