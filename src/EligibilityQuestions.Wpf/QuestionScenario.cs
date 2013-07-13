using System.Collections.Generic;

namespace EligibilityQuestions.Wpf
{
    /// <summary>
    /// Set Questions in your constructor
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class QuestionScenario<TModel> : IQuestionScenario where TModel : class, new()
    {
        private IEnumerable<Question> _questions;
        public IEnumerable<Question> Questions
        {
            get { return _questions; }
            set
            {
                _questions = value;
                if (_questions != null)
                {
                    foreach (var question in _questions)
                    {
                        question.Rank = QuestionRank.Primary;
                    }
                }
            }
        }

        public TModel BuildModel()
        {
            return ModelBuilder<TModel>.BuildModelFrom(Questions);
        }

        public string GetAnswerSummary()
        {
            return BuildModel().ToString();
        }
    }
}