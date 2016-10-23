#region Header
// --------------------------------------------------------------------
// Project:           Transitionz - WPF Animation extensions
// Description:       Collection of markup extensions allowing WPF animations to be applied to elements
// Copyright:		  Copyright © 2013-2016 SciChart Ltd & Transitionz Contributors
// License:           Apache v2.0 License https://www.apache.org/licenses/LICENSE-2.0
// --------------------------------------------------------------------
#endregion

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
