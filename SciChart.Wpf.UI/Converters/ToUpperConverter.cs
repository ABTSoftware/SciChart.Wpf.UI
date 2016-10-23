using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Wpf.UI.Converters
{
    public class ToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = value as string;
            if (input == null) return value;

            return input.ToUpper(CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}