using System;
using System.Windows;
using System.Windows.Media.Animation;
using SciChart.Wpf.UI.Transitionz.AttachedBehaviors;

namespace SciChart.Wpf.UI.Transitionz
{
    public partial class Transitionz
    {
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.RegisterAttached("Opacity", typeof(IOpacityParams), typeof(UI.Transitionz.Transitionz), new PropertyMetadata(default(IOpacityParams), OnOpacityChanged));

        public static void SetOpacity(UIElement element, IOpacityParams value)
        {
            element.SetValue(OpacityProperty, value);
        }

        public static IOpacityParams GetOpacity(UIElement element)
        {
            return (IOpacityParams)element.GetValue(OpacityProperty);
        }

        private static void OnOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldTransitionParams = e.OldValue as IOpacityParams;
            var newTransitionParams = e.NewValue as IOpacityParams;
            var target = d as FrameworkElement;
            if (target == null)
                return;

            if (oldTransitionParams != null)
            {
                target.Loaded -= OnLoadedForOpacity;
                target.DataContextChanged -= OnDataContextChangedForOpacity;
                Transitionz.RemoveVisibilityChangedHandler(target, OnVisibilityChangedForOpacity);
            }

            if (newTransitionParams != null)
            {
                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.Loaded) || Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.Once))
                {
                    target.Opacity = newTransitionParams.From;
                    target.Loaded += OnLoadedForOpacity;
                    if (target.IsLoaded()) OnLoadedForOpacity(target, null);
                }
                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.DataContextChanged))
                {
                    target.DataContextChanged += OnDataContextChangedForOpacity;
                }
                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.Visibility))
                {
                    Transitionz.AddVisibilityChangedHandler(target, OnVisibilityChangedForOpacity);
                }
            }
        }

        private static void OnVisibilityChangedForOpacity(object sender, EventArgs e)
        {
            var element = ((FrameworkElement)((PropertyChangeNotifier)sender).PropertySource);
            var visibility = Transitionz.GetVisibility(element);
            if (visibility == Visibility.Visible)
            {
                element.Visibility = Visibility.Visible;
            }
            element.BeginInvoke(() => DoOpacityTransition(GetOpacity(element), element, null, visibility), DispatchPriority.DataBind);
        }

        private static void OnDataContextChangedForOpacity(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            element.BeginInvoke(() => DoOpacityTransition(GetOpacity(element), element, null, null), DispatchPriority.DataBind);
        }

        private static void OnLoadedForOpacity(object sender, RoutedEventArgs routedEventArgs)
        {
            var element = ((FrameworkElement)sender);
            element.BeginInvoke(() => DoOpacityTransition(GetOpacity(element), element, OnLoadedForOpacity, null), DispatchPriority.DataBind);
        }

        private static void DoOpacityTransition(
            IOpacityParams transitionParams,
            FrameworkElement target,
            RoutedEventHandler onLoaded,
            Visibility? visibility)
        {
            if (onLoaded != null && Transitionz.HasFlag(transitionParams.TransitionOn, TransitionOn.Once))
            {
                target.Loaded -= onLoaded;
            }

            var reverse = Transitionz.IsVisibilityHidden(visibility);

            var a = new DoubleAnimation
            {
                From = reverse ? transitionParams.To : transitionParams.From,
                To = reverse ? transitionParams.From : transitionParams.To,
                FillBehavior = transitionParams.FillBehavior,
                BeginTime = TimeSpan.FromMilliseconds(transitionParams.BeginTime),
                Duration = new Duration(TimeSpan.FromMilliseconds(transitionParams.Duration)),
                EasingFunction = reverse ? (transitionParams.ReverseEase ?? transitionParams.Ease) : transitionParams.Ease,
                AutoReverse = transitionParams.AutoReverse,
            };

            if (visibility.HasValue)
                a.Completed += (_, __) => target.Visibility = visibility.Value;

            a.SetDesiredFrameRate(24);

            var storyboard = new Storyboard();
            storyboard.Children.Add(a);
            Storyboard.SetTarget(a, target);
            Storyboard.SetTargetProperty(a, new PropertyPath(UIElement.OpacityProperty));
            FreezeHelper.SetFreeze(storyboard, true);
            storyboard.Begin();
        }
    }
}