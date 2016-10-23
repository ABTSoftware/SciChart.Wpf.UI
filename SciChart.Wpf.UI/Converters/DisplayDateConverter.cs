using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Wpf.UI.Converters
{
    public class DisplayDateConverter : IValueConverter
    {
        public DisplayDateConverter()
        {
            FormattingString = "dd-MMM-yyyy";
        }

        public string FormattingString { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            return ((DateTime) value).ToString(FormattingString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}