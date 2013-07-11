using System.Collections.Generic;
using System.Linq;

namespace EligibilityQuestions
{
    public class ModelBuilder<TResult> where TResult : class, new()
    {
        private readonly IEnumerable<Question> _questions;

        public ModelBuilder(IEnumerable<Question> questions)
        {
            _questions = questions;
        }

        public TResult BuildModel()
        {
            var result = new TResult();
            _questions.SelectMany(x => x.AnsweredQuestions())
                .Select(x => new {x.Accessor, x.Answer})
                .Each(x => x.Accessor.SetValue(result, x.Answer));
            return result;
        }
        
    }
}