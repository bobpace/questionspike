using FubuCore.Reflection;

namespace EligibilityQuestions
{
    public interface IQuestion
    {
        Accessor Accessor { get; set; }
        string QuestionText { get; set; }
        string HelpText { get; set; }
    }
}