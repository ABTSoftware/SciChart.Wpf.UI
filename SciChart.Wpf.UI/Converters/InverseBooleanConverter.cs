using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Wpf.UI.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
