using System;
using System.Linq;
using System.Windows;

namespace SciChart.Wpf.UI.Controls.AttachedBehaviours.Transitionz
{
    public partial class Transitionz
    {
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached("Visibility", typeof(Visibility), typeof(Transitionz), new PropertyMetadata(default(Visibility)));

        public static void SetVisibility(UIElement element, Visibility value)
        {
            element.SetValue(VisibilityProperty, value);
        }

        public static Visibility GetVisibility(UIElement element)
        {
            return (Visibility)element.GetValue(VisibilityProperty);
        }

        private static bool HasFlag(TransitionOn transitionOn, TransitionOn flag)
        {
            return (transitionOn & flag) != 0;
        }

        public static void AddVisibilityChangedHandler(DependencyObject d, EventHandler handler)
        {
            var pcn = new PropertyChangeNotifier(d, Transitionz.VisibilityProperty);
            pcn.ValueChanged += handler;
        }

        public static void RemoveVisibilityChangedHandler(DependencyObject d, EventHandler handler)
        {
            if (d == null) throw new ArgumentNullException("d");

            var notifiers = PropertyChangeNotifier.GetNotifiers(d);
            if (notifiers == null) return;

            var pcn = notifiers.FirstOrDefault(n => n.PropertySource == d);
            if (pcn != null)
            {
                pcn.ValueChanged -= handler;
                pcn.Dispose();
                notifiers.Remove(pcn);
            }
        }

        private static bool IsVisibilityHidden(Visibility? visibility)
        {
            return visibility == Visibility.Collapsed
#if !SILVERLIGHT
                   || visibility == Visibility.Hidden
#endif
                ;
        }
    }
}