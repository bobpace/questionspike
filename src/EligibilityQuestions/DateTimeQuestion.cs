﻿namespace EligibilityQuestions
{
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