using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SciChart.Wpf.UI
{
    [ContentProperty("Content")]
    public class LabelControl : Control
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof (object), typeof (LabelControl), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof (string), typeof (LabelControl), new PropertyMetadata(default(string)));

        public LabelControl()
        {
            this.DefaultStyleKey = typeof (LabelControl);            
        }

        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public object Content
        {
            get { return (object) GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
    }
}
