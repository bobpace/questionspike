using System.Collections.Generic;

namespace EligibilityQuestions.Wpf
{
    public interface IQuestionScenario
    {
        IEnumerable<Question> Questions { get; set; }
        string GetAnswerSummary();
    }
}