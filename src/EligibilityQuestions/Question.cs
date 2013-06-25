using System;
using System.ComponentModel;
using ExtendHealth.Ssc.Framework;
using FubuCore;
using FubuCore.Reflection;

namespace EligibilityQuestions
{
    public delegate Question<TResult> NextQuestion<TResult>(Question<TResult> question);

    public abstract class Question<TResult> : NotifyPropertyChangedBase, IQuestion
    {
        protected NextQuestion<TResult> Done = (x => null);
        private object _answer;

        public Accessor Accessor { get; set; }
        public string QuestionText { get; set; }
        public string HelpText { get; set; }

        public object Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                NotifyOfPropertyChange(() => NextQuestion);
            }
        }

        public abstract NextQuestion<TResult> GetNextQuestion();

        public Question<TResult> NextQuestion
        {
            get
            {
                if (Answer == null) return null;
                return GetNextQuestion()(this);
            }
        }

        public void SetAnswer(TResult instance)
        {
            Accessor.SetValue(instance, Answer);
        }
    }
}