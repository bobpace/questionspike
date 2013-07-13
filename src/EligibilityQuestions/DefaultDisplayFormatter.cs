namespace EligibilityQuestions
{
    public class DefaultDisplayFormatter : IFlagsEnumDisplayFormatter
    {
        public string FormatValue(object value)
        {
            return value.ToString();
        }
    }
}