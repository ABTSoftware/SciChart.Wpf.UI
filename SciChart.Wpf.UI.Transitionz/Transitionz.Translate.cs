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
using SciChart.Wpf.UI.Transitionz.AttachedBehaviors;

namespace SciChart.Wpf.UI.Transitionz
{
    public partial class Transitionz
    {
        public static readonly DependencyProperty TranslateProperty =
            DependencyProperty.RegisterAttached("Translate", typeof(ITranslateParams), typeof(UI.Transitionz.Transitionz), new PropertyMetadata(default(ITranslateParams), OnTranslateParamsChanged));

        public static void SetTranslate(UIElement element, ITranslateParams value)
        {
            element.SetValue(TranslateProperty, value);
        }

        public static ITranslateParams GetTranslate(UIElement element)
        {
            return (ITranslateParams)element.GetValue(TranslateProperty);
        }

        private static void OnTranslateParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldTransitionParams = e.OldValue as ITranslateParams;
            var newTransitionParams = e.NewValue as ITranslateParams;
            var target = d as FrameworkElement;
            if (target == null)
                return;

            if (oldTransitionParams != null)
            {
                target.Loaded -= OnLoadedForTranslate;
                target.DataContextChanged -= OnDataContextChangedForTranslate;
                Transitionz.RemoveVisibilityChangedHandler(target, OnVisibilityChangedForTranslate);
            }

            if (newTransitionParams != null)
            {
                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.Loaded) || Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.Once))
                {
                    target.Loaded += OnLoadedForTranslate;
                    if (target.IsLoaded()) OnLoadedForTranslate(target, null);
                }
                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.DataContextChanged))
                {
                    target.DataContextChanged += OnDataContextChangedForTranslate;
                }
                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.Visibility))
                {
                    Transitionz.AddVisibilityChangedHandler(target, OnVisibilityChangedForTranslate);
                }
            }
        }

        private static void OnVisibilityChangedForTranslate(object sender, EventArgs e)
        {
            var element = ((FrameworkElement)((PropertyChangeNotifier)sender).PropertySource);
            var visibility = Transitionz.GetVisibility(element);
            if (visibility == Visibility.Visible)
            {
                element.Visibility = Visibility.Visible;
            }
            DoTranslateTransition(GetTranslate(element), element, null, visibility);
        }

        private static void OnDataContextChangedForTranslate(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            DoTranslateTransition(GetTranslate(element), element, null, null);
        }

        private static void OnLoadedForTranslate(object sender, RoutedEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            DoTranslateTransition(GetTranslate(element), element, OnLoadedForTranslate, null);
        }

        private static void DoTranslateTransition(ITranslateParams transitionParams, FrameworkElement target, RoutedEventHandler onLoaded, Visibility? visibility)
        {
            if (onLoaded != null && Transitionz.HasFlag(transitionParams.TransitionOn, TransitionOn.Once))
            {
                target.Loaded -= onLoaded;
            }
            var reverse = Transitionz.IsVisibilityHidden(visibility);
            var translateTransform = new TranslateTransform()
            {
                X = reverse ? transitionParams.To.X : transitionParams.From.X,
                Y = reverse ? transitionParams.To.Y : transitionParams.From.Y,
            };
            target.RenderTransform = translateTransform;

            var x = new DoubleAnimation
            {
                From = reverse ? transitionParams.To.X : transitionParams.From.X,
                To = reverse ? transitionParams.From.X : transitionParams.To.X,
                FillBehavior = transitionParams.FillBehavior,
                BeginTime = TimeSpan.FromMilliseconds(transitionParams.BeginTime),
                Duration = new Duration(TimeSpan.FromMilliseconds(transitionParams.Duration)),
                EasingFunction = reverse ? (transitionParams.ReverseEase ?? transitionParams.Ease) : transitionParams.Ease,
                AutoReverse = transitionParams.AutoReverse,
            };
            
            var y = new DoubleAnimation
            {
                From = reverse ? transitionParams.To.Y : transitionParams.From.Y,
                To = reverse ? transitionParams.From.Y : transitionParams.To.Y,
                FillBehavior = transitionParams.FillBehavior,
                BeginTime = TimeSpan.FromMilliseconds(transitionParams.BeginTime),
                Duration = new Duration(TimeSpan.FromMilliseconds(transitionParams.Duration)),
                EasingFunction = reverse ? (transitionParams.ReverseEase ?? transitionParams.Ease) : transitionParams.Ease,
                AutoReverse = transitionParams.AutoReverse,
            };

            // Directly adding RepeatBehavior to constructor breaks existing animations, so only add it if properly defined
            if ( transitionParams.RepeatBehavior == RepeatBehavior.Forever 
                || transitionParams.RepeatBehavior.HasDuration 
                || (transitionParams.RepeatBehavior.HasDuration && transitionParams.RepeatBehavior.Count > 0))
            {
                x.RepeatBehavior = transitionParams.RepeatBehavior;
                y.RepeatBehavior = transitionParams.RepeatBehavior;
            }

            if (visibility.HasValue)
                x.Completed += (_, __) => target.Visibility = visibility.Value;

            x.SetDesiredFrameRate(24);
            y.SetDesiredFrameRate(24);

            (target.RenderTransform).BeginAnimation(TranslateTransform.XProperty, x);
            (target.RenderTransform).BeginAnimation(TranslateTransform.YProperty, y);
        }
    }
}