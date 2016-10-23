using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SciChart.Wpf.UI.AttachedBehaviours
{
    /// <summary>
    /// Allows setting of Design-Time properties
    /// </summary>
    public class DesignMode
    {
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof (Brush), typeof (DesignMode), new PropertyMetadata(default(Brush), OnBackgroundChanged));
        
        public static void SetBackground(UIElement element, Brush value)
        {
            element.SetValue(BackgroundProperty, value);
        }

        public static Brush GetBackground(UIElement element)
        {
            return (Brush) element.GetValue(BackgroundProperty);
        }

        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as Control;
            if (element == null)
                return;

            if (DesignerProperties.GetIsInDesignMode(element))
            {
                element.Background = e.NewValue as Brush;
            }
        }
    }
}
