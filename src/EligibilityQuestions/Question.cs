using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExtendHealth.Ssc.Framework;
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

        public IEnumerable<Question> AnsweredQuestions()
        {
            var question = this;
            while (question != null)
            {
                yield return question;
                question = question.NextQuestion;
            }
        }

        //////////////////
        //Factory methods/
        //////////////////
        public static YesNoQuestion ForAnswer<TResult>(Expression<Func<TResult, bool?>> accessor)
        {
            return ForAnswer<TResult, YesNoQuestion, bool?>(accessor);
        }

        public static DateTimeQuestion ForAnswer<TResult>(Expression<Func<TResult, DateTime?>> accessor)
        {
            return ForAnswer<TResult, DateTimeQuestion, DateTime?>(accessor);
        }

        private static TQuestion ForAnswer<TResult, TQuestion, TProperty>(Expression<Func<TResult, TProperty>> accessor)
            where TQuestion : Question, new()
        {
            var question = new TQuestion
            {
                Accessor = accessor.ToAccessor()
            };
            return question;
        }
    }
}