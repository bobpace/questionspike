using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FubuCore.Reflection;

namespace EligibilityQuestions
{
    public interface IQuestion
    {
        Accessor Accessor { get; set; }
        string QuestionText { get; set; }
        string HelpText { get; set; }
    }

    public delegate Question<TResult> NextQuestion<TResult>(Question<TResult> question);

    public class YesNoQuestion<TResult> : Question<TResult>
    {
        private NextQuestion<TResult> _onYes;
        private NextQuestion<TResult> _onNo;

        public YesNoQuestion(Expression<Func<TResult, bool?>> accessor)
        {
            _onNo = Done;
            _onYes = Done;
            Accessor = accessor.ToAccessor();
        }

        public YesNoQuestion(Expression<Func<TResult, bool>> accessor)
        {
            _onNo = Done;
            _onYes = Done;
            Accessor = accessor.ToAccessor();
        }

        public YesNoQuestion<TResult> OnYes(NextQuestion<TResult> onYes)
        {
            _onYes = onYes;
            return this;
        }

        public YesNoQuestion<TResult> OnNo(NextQuestion<TResult> onNo)
        {
            _onNo = onNo;
            return this;
        }

        public override NextQuestion<TResult> GetNextQuestion()
        {
            return x =>
            {
                var answer = (bool) x.Answer;
                return answer ? _onYes(x) : _onNo(x);
            };
        }
    }

    public class DateTimeQuestion<TResult> : Question<TResult>
    {
        private NextQuestion<TResult> _onNext;

        public DateTimeQuestion(Expression<Func<TResult, DateTime>> accessor)
        {
            _onNext = Done;
            Accessor = accessor.ToAccessor();
        }

        public DateTimeQuestion(Expression<Func<TResult, DateTime?>> accessor)
        {
            _onNext = Done;
            Accessor = accessor.ToAccessor();
        }

        public DateTimeQuestion<TResult> OnNext(NextQuestion<TResult> onNext)
        {
            _onNext = onNext;
            return this;
        }

        public override NextQuestion<TResult> GetNextQuestion()
        {
            return x => _onNext(x);
        }
    }

    public abstract class Question<TResult> : IQuestion
    {
        protected NextQuestion<TResult> Done = (x => null);

        public Accessor Accessor { get; set; }
        public string QuestionText { get; set; }
        public string HelpText { get; set; }

        public object Answer { get; set; }

        public abstract NextQuestion<TResult> GetNextQuestion();

        public Question<TResult> AnswerQuestion(object answer)
        {
            Answer = answer;
            return GetNextQuestion()(this);
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

    public class OneByOneQuestionPresenter<T> : IQuestionPresenter
    {
        private readonly IQuestionProcessor<T> _questionProcessor;

        public OneByOneQuestionPresenter(IQuestionProcessor<T> questionProcessor)
        {
            _questionProcessor = questionProcessor;
        }
    }

    public interface IQuestionPresenter
    {
    }

}