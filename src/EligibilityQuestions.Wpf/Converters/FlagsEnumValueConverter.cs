using System;
using System.Globalization;
using System.Windows.Data;

namespace EligibilityQuestions.Wpf.Converters
{
    public class FlagsEnumValueConverter : IValueConverter
    {
        private readonly Type _flagsEnumType;
        private int? _targetValue;

        /// <summary>
        /// To be used with the FlagsEnumCheckBoxPanel, otherwise use at own risk
        /// Because this converter holds state, and normally converters are singletons
        /// this will be problematic to your usage of it outside of how FlagsEnumCheckBoxPanel uses it
        /// </summary>
        /// <param name="flagsEnumType"></param>
        public FlagsEnumValueConverter(Type flagsEnumType)
        {
            _flagsEnumType = flagsEnumType;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mask = (int) parameter;
            _targetValue = value != null
                ? (int?) ((int) value)
                : null;
            return _targetValue.HasValue && mask.HasFlag(_targetValue.Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool) value;
            if (isChecked && _targetValue == null)
            {
                _targetValue = 0;
            }
            _targetValue ^= (int) parameter;
            if (_targetValue == 0) return null;
            return Enum.Parse(_flagsEnumType, _targetValue.ToString());
        }
    }
}