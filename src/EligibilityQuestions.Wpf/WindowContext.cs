using System.Collections.Generic;

namespace EligibilityQuestions.Wpf
{
    public class WindowContext : NotifyPropertyChangedBase
    {
        private IEnumerable<Question> _questions;
        private string _answerSummary;

        public IEnumerable<Question> Questions
        {
            get { return _questions; }
            set
            {
                _questions = value;
                NotifyOfPropertyChange(() => Questions);
            }
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