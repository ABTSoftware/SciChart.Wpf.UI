#region Header
// --------------------------------------------------------------------
// Project:           Transitionz - WPF Animation extensions
// Description:       Collection of markup extensions allowing WPF animations to be applied to elements
// Copyright:		  Copyright © 2013-2016 SciChart Ltd & Transitionz Contributors
// License:           Apache v2.0 License https://www.apache.org/licenses/LICENSE-2.0
// --------------------------------------------------------------------
#endregion

using System;
using System.Linq;
using System.Windows;
using SciChart.Wpf.UI.Transitionz.AttachedBehaviors;

namespace SciChart.Wpf.UI.Transitionz
{
    public partial class Transitionz
    {
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached("Visibility", typeof(Visibility), typeof(UI.Transitionz.Transitionz), new PropertyMetadata(default(Visibility)));

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
            var pcn = new PropertyChangeNotifier(d, UI.Transitionz.Transitionz.VisibilityProperty);
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