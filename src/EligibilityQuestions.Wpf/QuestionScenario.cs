using System;
using System.Collections.Generic;
using System.Linq;

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

        public virtual IEnumerable<Question> AllQuestions { get; set; }

        public virtual IEnumerable<object> ScenarioModels
        {
            get { yield break; }
        }

        public TModel BuildModel()
        {
            var result = new TModel();
            _questions.SelectMany(x => x.AnsweredQuestions())
                .Select(x => new {x.Accessor, x.Answer})
                .Each(x => x.Accessor.SetValue(result, x.Answer));
            return result;
        }

        public TModel EmptyModel()
        {
            return new TModel();
        }

        public void Reset()
        {
            if (AllQuestions != null)
            {
                AllQuestions.Each(x => x.Answer = null);
            }
            SetAnswersFromModel(EmptyModel());
        }

        public void SetAnswersFromModel(object model)
        {
            SetAnswersFromModel(_questions, model);
        }

        private void SetAnswersFromModel(IEnumerable<Question> questions, object model)
        {
            questions = questions.ToList();
            if (!questions.Any())
                return;

            questions.Each(x => x.SetAnswerFromModel(model));
            var newlyRevealedQuestions = questions.SelectMany(x =>
            {
                var result = x.ExtraQuestions();
                var nextQuestion = x.NextQuestion;
                if (nextQuestion != null)
                {
                    result = result.Concat(new[] {nextQuestion});
                }
                return result;
            });
            SetAnswersFromModel(newlyRevealedQuestions, model);
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

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}