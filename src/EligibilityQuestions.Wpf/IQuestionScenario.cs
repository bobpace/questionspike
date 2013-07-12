using System.Collections.Generic;

namespace EligibilityQuestions.Wpf
{
    public interface IQuestionScenario
    {
        string GetAnswerSummary();
        IEnumerable<Question> Questions { get; }
    }
}