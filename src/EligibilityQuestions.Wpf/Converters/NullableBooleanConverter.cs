using System;
using System.Globalization;
using System.Windows.Data;

namespace EligibilityQuestions.Wpf.Converters
{
    public class NullableBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolean = (bool?) value;
            return boolean.HasValue && boolean.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}