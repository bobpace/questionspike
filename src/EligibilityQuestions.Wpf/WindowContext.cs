using ExtendHealth.Ssc.Framework;

namespace EligibilityQuestions.Wpf
{
    public class WindowContext : NotifyPropertyChangedBase
    {
        private Question _question;
        private string _answerSummary;

        public Question Question
        {
            get { return _question; }
            set
            {
                _question = value;
                NotifyOfPropertyChange(() => Question);
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