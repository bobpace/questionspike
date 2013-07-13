using System.Collections.Generic;

namespace EligibilityQuestions.Wpf
{
    /// <summary>
    /// Set Questions in your constructor
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class QuestionScenario<TModel> : NotifyPropertyChangedBase, IQuestionScenario where TModel : class, new()
    {
        private IEnumerable<Question> _questions;
        private string _answerSummary;

        public virtual IEnumerable<Question> Questions
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
                NotifyOfPropertyChange(() => Questions);
            }
        }

        public TModel BuildModel()
        {
            return ModelBuilder<TModel>.BuildModelFrom(Questions);
        }

        public string GetAnswerSummary()
        {
            AnswerSummary = BuildModel().ProperetyValuesToString();
            return AnswerSummary;
        }

        public string AnswerSummary
        {
            get { return _answerSummary; }
            set
            {
                _answerSummary = value;
                NotifyOfPropertyChange(() => AnswerSummary);
            }
        }
    }
}