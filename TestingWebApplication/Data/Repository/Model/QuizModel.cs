namespace TestingWebApplication.Data.Repository.Model
{
    using System.Collections.Generic;

    public class QuizModel
    {
        public string Title { get; set; }

        public long TotalTimeSecs { get; set; }

        public IList<QuizBlockModel> QuizBlocks { get; set; }
    }
}
