namespace TestingWebApplication.Models.AdminQuizResults
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модель для передачи данных о блоке теста.
    /// </summary>
    public class ResultQuizBlockViewModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ResultQuizBlockViewModel"/>.
        /// </summary>
        public ResultQuizBlockViewModel()
        {
            Answers = new List<ResultAnswerBlockViewModel>();
        }

        /// <summary>
        /// Получает или задает идентификатор блока вопроса.
        /// </summary>
        public long BlockId { get; set; }

        /// <summary>
        /// Получает или задает блок вопроса.
        /// </summary>
        [Required]
        [Display(Name = "Описание вопроса")]
        public ResultQuestionBlockViewModel Question { get; set; }

        /// <summary>
        /// Получает или задает коллекцию блоков ответов.
        /// </summary>
        public List<ResultAnswerBlockViewModel> Answers { get; set; }
    }
}
