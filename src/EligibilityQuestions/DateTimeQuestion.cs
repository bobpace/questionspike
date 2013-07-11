using System;
using System.Collections.Generic;

namespace EligibilityQuestions
{
    public class MultipleSelectQuestion : Question
    {
        private NextQuestion _onNext;

        public override NextQuestion GetNextQuestion()
        {
            return x => _onNext(x);
        }

        public MultipleSelectQuestion OnNext(NextQuestion onNext)
        {
            _onNext = onNext;
            return this;
        }

        public Type FlagsEnumType
        {
            get { return Accessor.PropertyType.UnwrapNullable(); }
        }

        public override object Answer
        {
            get
            {
                return base.Answer;
            }
            set
            {
                base.Answer = value;
            }
        }
    }

    public class DateTimeQuestion : Question
    {
        private NextQuestion _onNext;

        public DateTimeQuestion()
        {
            _onNext = Done;
        }

        public DateTimeQuestion OnNext(NextQuestion onNext)
        {
            _onNext = onNext;
            return this;
        }

        public override NextQuestion GetNextQuestion()
        {
            return x => _onNext(x);
        }
    }
}