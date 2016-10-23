using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SciChart.Wpf.UI.Controls.Converters
{
    public class VisibleIfNullConverter : IValueConverter
    {
        public bool Reverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nullResult = Reverse ? Visibility.Collapsed : Visibility.Visible;
            var notNullResult = Reverse ? Visibility.Visible : Visibility.Collapsed;

            return value == null ? nullResult : notNullResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibleIfNotNullConverter : NullToVisibilityConverter
    {
        public VisibleIfNotNullConverter()
        {
            ValueIfNotNull = Visibility.Visible;
            ValueIfNull = Visibility.Collapsed;
        }
    }

    public class NullToVisibilityConverter : IValueConverter
    {
        public NullToVisibilityConverter()
        {
            
        }

        public Visibility ValueIfNull { get; set; }
        public Visibility ValueIfNotNull { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? ValueIfNull : ValueIfNotNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringNullOrEmptyToVisibilityConverter : IValueConverter
    {
        public StringNullOrEmptyToVisibilityConverter()
        {

        }

        public Visibility ValueIfNullOrEmpty { get; set; }
        public Visibility ValueIfNotNullOrEmpty { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? ValueIfNullOrEmpty : ValueIfNotNullOrEmpty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
