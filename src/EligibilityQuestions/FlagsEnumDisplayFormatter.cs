using System.Collections.Generic;

namespace EligibilityQuestions
{
    public class FlagsEnumDisplayFormatter<TFlagsEnum> : IFlagsEnumDisplayFormatter
    {
        private readonly IDictionary<TFlagsEnum, string> _displayTextMap;

        public FlagsEnumDisplayFormatter(IDictionary<TFlagsEnum, string> displayTextMap)
        {
            _displayTextMap = displayTextMap;
        }

        public string FormatValue(object value)
        {
            var flagsEnum = (TFlagsEnum) value;
            return _displayTextMap.ContainsKey(flagsEnum)
                ? _displayTextMap[flagsEnum]
                : flagsEnum.ToString();
        }
    }
}