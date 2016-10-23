using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Wpf.UI.Converters
{
    public class NullToBooleanConverter : IValueConverter
    {
        public NullToBooleanConverter()
        {
            NotNullValue = true;
        }

        public bool NullValue { get; set; }
        public bool NotNullValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? NullValue : NotNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}