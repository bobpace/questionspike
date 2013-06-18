using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FubuCore;
using FubuCore.Reflection;

namespace EligibilityQuestions
{

    public class EndResultModel
    {
        public bool LikesBlue { get; set; }
        public bool? LikesGreen { get; set; }
        public DateTime Birthday { get; set; }
    }

    public interface IQuestion
    {
        Accessor Accessor { get; set; }
        string QuestionText { get; set; }
        string HelpText { get; set; }
    }

    public class Question<TResult> : IQuestion
    {
        private object _answer;

        public Question(Expression<Func<TResult,object>> accessor)
        {
            Accessor = accessor.ToAccessor();
        }

        public Accessor Accessor { get; set; }
        public string QuestionText { get; set; }
        public string HelpText { get; set; }

        public object Answer
        {
            get { return _answer; }
            set
            {
                ValidateSetWillSucceed(value.GetType());
                _answer = value;
            }
        }

        private void ValidateSetWillSucceed(Type valueType)
        {
            Action<Type> errorIfIncompatible = type =>
            {
                if (type != valueType)
                {
                    throw new ArgumentException("wrong property type for answer");
                }
            };

            errorIfIncompatible(Accessor.PropertyType.IsNullable()
                ? Accessor.PropertyType.GetGenericArguments().First()
                : Accessor.PropertyType);
        }

        public void SetAnswer(TResult instance)
        {
            Accessor.SetValue(instance, Answer);
        }
    }

    public interface IQuestionProcessor<TResult>
    {
        IEnumerable<IQuestion> Questions { get; set; }
        void Process();
    }


    public class DefaultQuestionProcessor<TResult> : IQuestionProcessor<TResult>
    {
        public DefaultQuestionProcessor(IEnumerable<IQuestion> questions)
        {
            Questions = questions;
        }

        public IEnumerable<IQuestion> Questions { get; set; }

        public void Process()
        {
            throw new NotImplementedException();
        }
    }

    public class OneByOneQuestionPresenter : IQuestionPresenter
    {
        private readonly IQuestionProcessor<EndResultModel> _questionProcessor;

        public OneByOneQuestionPresenter(IQuestionProcessor<EndResultModel> questionProcessor)
        {
            _questionProcessor = questionProcessor;
        }
    }

    public interface IQuestionPresenter
    {
    }

}