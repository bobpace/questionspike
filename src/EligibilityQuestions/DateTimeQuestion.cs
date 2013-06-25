using System;
using System.Linq.Expressions;

namespace EligibilityQuestions
{
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
}