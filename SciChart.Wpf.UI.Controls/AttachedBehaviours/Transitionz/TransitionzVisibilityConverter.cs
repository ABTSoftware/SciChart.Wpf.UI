using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SciChart.Wpf.UI.Controls.AttachedBehaviours.Transitionz
{
    public class TransitionzVisibilityConverter : IValueConverter
    {
        public TransitionzVisibilityConverter()
        {
            FadeInTransition = new OpacityParams() {Duration = 500, From = 0, To = 1};
            FadeOutTransition = new OpacityParams() { Duration = 0, From = 1, To = 0 };
        }

        public IOpacityParams FadeInTransition { get; set; }
        public IOpacityParams FadeOutTransition { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Visibility) value) == Visibility.Visible ? FadeInTransition : FadeOutTransition;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
