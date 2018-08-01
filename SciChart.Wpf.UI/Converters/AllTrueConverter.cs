using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using SciChart.UI.Reactive.Extensions;

namespace SciChart.Wpf.UI.Converters
{
    public class AllTrueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.IsNullOrEmpty()) return false;

            return values.All(x => x is Boolean && ((bool)x) == true);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
