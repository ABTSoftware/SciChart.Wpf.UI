using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SciChart.Wpf.UI
{
    [ContentProperty("Content")]
    public class BusyPanel : Control
    {
        public static readonly DependencyProperty BusyMessageProperty = DependencyProperty.Register("BusyMessage", typeof (string), typeof (BusyPanel), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof (object), typeof (BusyPanel), new PropertyMetadata(default(object)));

        public BusyPanel()
        {
            DefaultStyleKey = typeof (BusyPanel);
        }

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public string BusyMessage
        {
            get { return (string) GetValue(BusyMessageProperty); }
            set { SetValue(BusyMessageProperty, value); }
        }
    }
}