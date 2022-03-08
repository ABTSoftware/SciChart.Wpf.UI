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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using SciChart.Wpf.UI.Transitionz.AttachedBehaviors;

namespace SciChart.Wpf.UI.Transitionz
{
    public partial class Transitionz
    {
        public static readonly DependencyProperty BlurProperty = DependencyProperty.RegisterAttached(
            "Blur", typeof(IBlurParams), typeof(Transitionz), new PropertyMetadata(default(IBlurParams), OnBlurParamsChanged));

        public static void SetBlur(DependencyObject element, IBlurParams value)
        {
            element.SetValue(BlurProperty, value);
        }

        public static IBlurParams GetBlur(DependencyObject element)
        {
            return (IBlurParams)element.GetValue(BlurProperty);
        }

        private static void OnBlurParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldTransitionParams = e.OldValue as IBlurParams;
            var newTransitionParams = e.NewValue as IBlurParams;
            var target = d as FrameworkElement;
            if (target == null)
                return;

            if (oldTransitionParams != null)
            {
                target.Loaded -= Transitionz.OnLoadedForTranslate;
                target.DataContextChanged -= Transitionz.OnDataContextChangedForTranslate;
            }

            if (newTransitionParams != null)
            {
                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.Loaded))
                {
                    target.Loaded += OnLoadedForBlur;
                    if (target.IsLoaded()) OnLoadedForBlur(target, null);
                }
                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.DataContextChanged))
                {
                    target.DataContextChanged += OnDataContextChangedForBlur;
                }
                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.Visibility))
                {
                    Transitionz.AddVisibilityChangedHandler(target, OnVisibilityChangedForBlur);
                }
            }
        }

        private static void OnVisibilityChangedForBlur(object sender, EventArgs e)
        {
            var element = ((FrameworkElement)((PropertyChangeNotifier)sender).PropertySource);
            var visibility = Transitionz.GetVisibility(element);
            if (visibility == Visibility.Visible)
            {
                element.Visibility = Visibility.Visible;
            }
            DoBlurTansition(GetBlur(element), element, null, visibility);
        }

        private static void OnLoadedForBlur(object sender, RoutedEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            DoBlurTansition(GetBlur(element), element, OnLoadedForBlur, null);
        }

        private static void OnDataContextChangedForBlur(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            DoBlurTansition(GetBlur(element), element, null, null);
        }

        private static void DoBlurTansition(
            IBlurParams blurParams, 
            FrameworkElement target, 
            RoutedEventHandler onLoaded,
            Visibility? visibility)
        {
            var reverse = Transitionz.IsVisibilityHidden(visibility);
            var blurEffect = new BlurEffect() { Radius = reverse ? blurParams.To : blurParams.From };
            target.Effect = blurEffect;

            if (onLoaded != null && Transitionz.HasFlag(blurParams.TransitionOn, TransitionOn.Once))
            {
                target.Loaded -= onLoaded;
            }            

            var a = new DoubleAnimation
            {
                From = reverse ? blurParams.To : blurParams.From,
                To = reverse ? blurParams.From : blurParams.To,
                FillBehavior = blurParams.FillBehavior,
                BeginTime = TimeSpan.FromMilliseconds(blurParams.BeginTime),
                Duration = new Duration(TimeSpan.FromMilliseconds(blurParams.Duration)),
                EasingFunction = reverse ? (blurParams.ReverseEase ?? blurParams.Ease) : blurParams.Ease,
                AutoReverse = blurParams.AutoReverse,
            };

            // Directly adding RepeatBehavior to constructor breaks existing animations, so only add it if properly defined
            if (blurParams.RepeatBehavior == RepeatBehavior.Forever
                || blurParams.RepeatBehavior.HasDuration
                || (blurParams.RepeatBehavior.HasDuration && blurParams.RepeatBehavior.Count > 0))
            {
                a.RepeatBehavior = blurParams.RepeatBehavior;
            }

            if (blurParams.To == 0.0)
            {
                a.Completed += (_, __) => target.Effect = null;
            }

            if (visibility.HasValue)
                a.Completed += (_, __) => target.Visibility = visibility.Value;

            var storyboard = new Storyboard();
            storyboard.Children.Add(a);
            Storyboard.SetTarget(a, ((BlurEffect)target.Effect));
            Storyboard.SetTargetProperty(a, new PropertyPath(BlurEffect.RadiusProperty));
            FreezeHelper.SetFreeze(storyboard, true);
            storyboard.Begin();
        }
    }
}