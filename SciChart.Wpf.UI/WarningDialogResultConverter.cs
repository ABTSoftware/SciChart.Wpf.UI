using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SciChart.Wpf.UI.Controls
{
    public class WarningDialogResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = parameter as string;
            if (param == null) return Visibility.Collapsed;

            WarningDialogResult res;
            if (!Enum.TryParse(param, true, out res)) return Visibility.Collapsed;

            return ((WarningDialogResult)value).HasFlag(res) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}