using System.Linq;

namespace EligibilityQuestions.Wpf
{
    public class ExistingQuestionScenario : QuestionScenario<ExistingModel>
    {
        public ExistingQuestionScenario()
        {
            Questions = Enumerable.Empty<Question>();
        }
    }
}