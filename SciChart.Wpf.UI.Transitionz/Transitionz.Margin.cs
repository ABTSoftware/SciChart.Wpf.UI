#region Header
// --------------------------------------------------------------------
// Project:           Transitionz - WPF Animation extensions
// Description:       Collection of markup extensions allowing WPF animations to be applied to elements
// Copyright:		  Copyright © 2013-2016 SciChart Ltd & Transitionz Contributors
// License:           Apache v2.0 License https://www.apache.org/licenses/LICENSE-2.0
// --------------------------------------------------------------------
#endregion

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SciChart.Wpf.UI.Transitionz
{
    // Five issues remain 
    // 
    // e.Handled in VisibilityChanged event
    // ThicknessAnimation doesn't exist
    // LayoutTransform doesn't exist
    // Cannot declare EventManager.RegisterRoutedEvent
    public partial class Transitionz
    {               
       public static readonly DependencyProperty MarginProperty =
            DependencyProperty.RegisterAttached("Margin", typeof(MarginParamsExtension), typeof(UI.Transitionz.Transitionz), new PropertyMetadata(default(MarginParamsExtension), OnMarginParamsChanged));

        public static void SetMargin(UIElement element, MarginParamsExtension value)
        {
            element.SetValue(MarginProperty, value);
        }

        public static MarginParamsExtension GetMargin(UIElement element)
        {
            return (MarginParamsExtension)element.GetValue(MarginProperty);
        }

        private static void OnMarginParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var transitionParams = e.NewValue as MarginParamsExtension;
            var target = d as FrameworkElement;
            if (transitionParams == null || target == null)
                return;
            
            target.Margin = transitionParams.From;
            RoutedEventHandler onLoaded = null;
            onLoaded = (_, __) => target.BeginInvoke(() =>
            {
                target.Loaded -= onLoaded;
                var a = new ThicknessAnimation
                {
                    From = transitionParams.From,
                    To = transitionParams.To,
                    FillBehavior = transitionParams.FillBehavior,
                    BeginTime = TimeSpan.FromMilliseconds(transitionParams.BeginTime),
                    Duration = new Duration(TimeSpan.FromMilliseconds(transitionParams.Duration)),
                    EasingFunction = transitionParams.Ease
                };


                // Directly adding RepeatBehavior to constructor breaks existing animations, so only add it if properly defined
                if (transitionParams.RepeatBehavior == RepeatBehavior.Forever
                    || transitionParams.RepeatBehavior.HasDuration
                    || (transitionParams.RepeatBehavior.HasDuration && transitionParams.RepeatBehavior.Count > 0))
                {
                    a.RepeatBehavior = transitionParams.RepeatBehavior;
                }
                var storyboard = new Storyboard();

                storyboard.Children.Add(a);
                Storyboard.SetTarget(a, target);
                Storyboard.SetTargetProperty(a, new PropertyPath(FrameworkElement.MarginProperty));
                storyboard.Begin();
            }, DispatchPriority.DataBind);

            if (target.IsLoaded())
                onLoaded(null, null);
            else
                target.Loaded += onLoaded;
        }
      
    }
}
