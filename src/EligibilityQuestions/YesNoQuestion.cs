using System;
using System.Linq.Expressions;

namespace EligibilityQuestions
{
    public class YesNoQuestion : Question
    {
        private NextQuestion _onYes;
        private NextQuestion _onNo;

        public YesNoQuestion()
        {
            _onNo = Done;
            _onYes = Done;
        }

        public YesNoQuestion ForAnswer<TResult>(Expression<Func<TResult, bool?>> accessor)
        {
            Accessor = accessor.ToAccessor();
            return this;
        }

        public YesNoQuestion OnYes(NextQuestion onYes)
        {
            _onYes = onYes;
            return this;
        }

        public YesNoQuestion OnNo(NextQuestion onNo)
        {
            _onNo = onNo;
            return this;
        }

        public override NextQuestion GetNextQuestion()
        {
            return x =>
            {
                var answer = (bool) x.Answer;
                return answer ? _onYes(x) : _onNo(x);
            };
        }
    }
}