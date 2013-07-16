using System;
using System.Collections.Generic;
using System.Linq;

namespace EligibilityQuestions
{
    public class MultipleSelectQuestion : Question, IFlagsEnumFormatterProvider
    {
        private NextQuestion _onNext;
        private IDictionary<object, Question> _questionMap;

        public MultipleSelectQuestion()
        {
            _onNext = Done;
            DisplayFormatter = new PascalCasingSpacesDisplayFormatter();
        }

        public override NextQuestion GetNextQuestion()
        {
            return x => _onNext(x);
        }

        public MultipleSelectQuestion OnNext(NextQuestion onNext)
        {
            _onNext = onNext;
            return this;
        }

        public MultipleSelectQuestion SetExtraQuestions<TFlagsEnum>(IDictionary<TFlagsEnum, Question> questionMap)
        {
            if (typeof (TFlagsEnum) != FlagsEnumType)
            {
                throw new ArgumentException("use the same flags enum type");
            }
            _questionMap = questionMap.Keys
                .Select(x => new {orig = x, intValue = Convert.ChangeType(x, TypeCode.Int32)})
                .ToDictionary(x => x.intValue, x => questionMap[x.orig]);

            return this;
        }

        public IFlagsEnumDisplayFormatter DisplayFormatter { get; set; }

        public Type FlagsEnumType
        {
            get { return Accessor.PropertyType.UnwrapNullable(); }
        }

        public IEnumerable<Question> NextQuestions
        {
            get
            {
                if (Answer != null && _questionMap != null)
                {
                    var currentAnswer = (int) Answer;
                    return _questionMap
                        .Where(x => currentAnswer.HasFlag((int)x.Key))
                        .Select(x => x.Value);
                }
                return Enumerable.Empty<Question>();
            }
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
                NotifyOfPropertyChange(() => NextQuestions);
            }
        }

        public override IEnumerable<Question> ExtraQuestions()
        {
            return NextQuestions;
        }
    }
}