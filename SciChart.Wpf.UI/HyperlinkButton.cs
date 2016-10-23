using System.Windows;
using System.Windows.Controls;

namespace SciChart.Wpf.UI
{
    public class HyperlinkButton : Button
    {
        public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(
            "NavigateUri", typeof (string), typeof (HyperlinkButton), new PropertyMetadata(default(string)));

        public string NavigateUri
        {
            get { return (string) GetValue(NavigateUriProperty); }
            set { SetValue(NavigateUriProperty, value); }
        }

        public string TargetName
        {
            get;
            set;
        }
    }
}
