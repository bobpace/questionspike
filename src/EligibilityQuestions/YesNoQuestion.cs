using System;
using System.Linq.Expressions;

namespace EligibilityQuestions
{
    public class YesNoQuestion
    {
        public string QuestionText { get; set; }
    }

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
}