using System.Collections.Generic;

namespace EligibilityQuestions.Wpf
{
    public abstract class QuestionScenario<TModel> : IQuestionScenario where TModel : class, new()
    {
        public IEnumerable<Question> Questions { get; set; }

        public TModel BuildModel()
        {
            return new ModelBuilder<TModel>(Questions).BuildModel();
        }

        public string GetAnswerSummary()
        {
            return BuildModel().ToString();
        }
    }
}