using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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

        public static YesNoQuestion ForAnswer<TResult>(Expression<Func<TResult, bool?>> accessor)
        {
            var question = new YesNoQuestion
            {
                Accessor = accessor.ToAccessor()
            };
            return question;
        }

        public static DateTimeQuestion ForAnswer<TResult>(Expression<Func<TResult, DateTime?>> accessor)
        {
            var question = new DateTimeQuestion
            {
                Accessor = accessor.ToAccessor()
            };
            return question;
        }

        public Question NextQuestion
        {
            get
            {
                if (Answer == null) return null;
                var question = GetNextQuestion()(this);
                return question;
            }
        }

        public IEnumerable<Question> AnsweredQuestions()
        {
            var question = this;
            while (question != null)
            {
                yield return question;
                question = question.NextQuestion;
            }
        }
    }

    public class ModelBuilder<TResult> where TResult : class, new()
    {
        private readonly Question _firstQuestion;

        public ModelBuilder(Question firstQuestion)
        {
            _firstQuestion = firstQuestion;
        }

        public TResult BuildModel()
        {
            var result = new TResult();

            _firstQuestion.AnsweredQuestions()
                .Select(x => new {x.Accessor, x.Answer})
                .Each(x => x.Accessor.SetValue(result, x.Answer));
            return result;
        }
        
    }
}