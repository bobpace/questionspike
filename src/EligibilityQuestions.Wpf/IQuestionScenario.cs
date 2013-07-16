using System.Collections.Generic;

namespace EligibilityQuestions.Wpf
{
    public interface IQuestionScenario
    {
        IEnumerable<Question> Questions { get; set; }
        IEnumerable<object> ScenarioModels { get; }
        string GetAnswerSummary();
        void Reset();
        void SetAnswersFromModel(object model);
    }
}