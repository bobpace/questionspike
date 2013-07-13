using System.Text.RegularExpressions;

namespace EligibilityQuestions
{
    public class PascalCasingSpacesDisplayFormatter : IFlagsEnumDisplayFormatter
    {
        private readonly Regex _replaceRegex;

        public PascalCasingSpacesDisplayFormatter()
        {
            _replaceRegex = new Regex(@"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", RegexOptions.Compiled);
        }

        public string FormatValue(object value)
        {
            return _replaceRegex.Replace(value.ToString(), " $1");
        }
    }
}