using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Wpf.UI.Controls.Converters
{
    public class BoolToValueConverter : IValueConverter
    {
        public object TrueValue { get; set; }
        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            return ((bool?) value == true) ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullableBoolToValueConverter : IValueConverter
    {
        public object NullValue { get; set; }
        public object TrueValue { get; set; }
        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? b = (bool?) value;
            return b.HasValue ? b.Value ? TrueValue : FalseValue : NullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}