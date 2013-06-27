using System.Collections.Generic;
using System.Linq;

namespace EligibilityQuestions
{
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