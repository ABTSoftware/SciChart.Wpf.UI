using System.Windows;

namespace SciChart.Wpf.UI.Transitionz.AttachedBehaviors
{
    public class FreezeHelper
    {
        public static readonly DependencyProperty FreezeProperty =
            DependencyProperty.RegisterAttached("Freeze", typeof(bool), typeof(FreezeHelper), new PropertyMetadata(false, OnFreezePropertyChanged));

        private static void OnFreezePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if !SILVERLIGHT
            // No Freezable in Silverlight 
            var f = d as Freezable;
            if (f != null)
            {
                if (true.Equals(e.NewValue) && f.CanFreeze)
                    f.Freeze();
            }
#endif
        }

        public static void SetFreeze(DependencyObject element, bool value)
        {
            element.SetValue(FreezeProperty, value);
        }

        public static bool GetFreeze(DependencyObject element)
        {
            return (bool)element.GetValue(FreezeProperty);
        }
    }
}
