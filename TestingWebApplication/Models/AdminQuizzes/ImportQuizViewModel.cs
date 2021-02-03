namespace TestingWebApplication.Models.AdminQuizzes
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Модель для передачи данных о импортируемом тесте.
    /// </summary>
    public class ImportQuizViewModel
    {
        /// <summary>
        /// Получает или задает импортируемый файл.
        /// </summary>
        [Display(Name = "Файл")]
        public IFormFile File { get; set; }
    }
}
