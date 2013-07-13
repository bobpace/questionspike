using System.Text.RegularExpressions;

namespace EligibilityQuestions
{
    public class PascalCasingSpacesDisplayFormatter : IFlagsEnumDisplayFormatter
    {
        private readonly Regex _replaceRegex;

        public PascalCasingSpacesDisplayFormatter()
        {
            _replaceRegex = new Regex(@"(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", RegexOptions.Compiled);
        }

        public string FormatValue(object value)
        {
            return _replaceRegex.Replace(value.ToString(), " $1");
        }
    }
}