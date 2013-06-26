using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ExtendHealth.Ssc.Framework;
using FubuCore;
using FubuCore.Reflection;

namespace EligibilityQuestions
{
    public delegate Question NextQuestion(Question question);

    public abstract class Question : NotifyPropertyChangedBase
    {
        protected NextQuestion Done = (x => null);
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

        public abstract NextQuestion GetNextQuestion();

        public Question NextQuestion
        {
            get
            {
                if (Answer == null) return null;
                var question = GetNextQuestion()(this);
                return question;
            }
        }
    }

    public class ModelBuilder<TResult> where TResult : class, new()
    {
        private readonly IEnumerable<Question> _questions;

        public ModelBuilder(IEnumerable<Question> questions)
        {
            _questions = questions;
        }

        public TResult BuildModel()
        {
            var result = new TResult();
            _questions
                .Select(x => new {x.Accessor, x.Answer})
                .Each(x => x.Accessor.SetValue(result, x.Answer));
            return result;
        }
        
    }
}