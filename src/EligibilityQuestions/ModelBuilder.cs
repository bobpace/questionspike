using System.Collections.Generic;
using System.Linq;

namespace EligibilityQuestions
{
    public static class ModelBuilder<TModel> where TModel : class, new()
    {
        public static TModel BuildModelFrom(IEnumerable<Question> questions)
        {
            var result = new TModel();
            questions.SelectMany(x => x.AnsweredQuestions())
                .Select(x => new {x.Accessor, x.Answer})
                .Each(x => x.Accessor.SetValue(result, x.Answer));
            return result;
        }
        
    }
}