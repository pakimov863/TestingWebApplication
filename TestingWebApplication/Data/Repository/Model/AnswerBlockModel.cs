namespace TestingWebApplication.Data.Repository.Model
{
    using Shared;

    public class AnswerBlockModel
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public AnswerBlockType AnswerType { get; set; }

        public bool IsCorrect { get; set; }
    }
}
