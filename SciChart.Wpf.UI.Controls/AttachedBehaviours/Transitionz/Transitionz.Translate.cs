using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SciChart.Wpf.UI.Controls.AttachedBehaviours.Transitionz
{
    public partial class Transitionz
    {
        public static readonly DependencyProperty TranslateProperty =
            DependencyProperty.RegisterAttached("Translate", typeof(ITranslateParams), typeof(Transitionz), new PropertyMetadata(default(ITranslateParams), OnTranslateParamsChanged));

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
                RemoveVisibilityChangedHandler(target, OnVisibilityChangedForTranslate);
            }

            if (newTransitionParams != null)
            {
                var translateTransform = new TranslateTransform() { X = newTransitionParams.From.X, Y = newTransitionParams.From.Y };
                target.RenderTransform = translateTransform;
                if (HasFlag(newTransitionParams.TransitionOn, TransitionOn.Loaded) || HasFlag(newTransitionParams.TransitionOn, TransitionOn.Once))
                {
                    target.Loaded += OnLoadedForTranslate;
                    if (target.IsLoaded()) OnLoadedForTranslate(target, null);
                }
                if (HasFlag(newTransitionParams.TransitionOn, TransitionOn.DataContextChanged))
                {
                    target.DataContextChanged += OnDataContextChangedForTranslate;
                }
                if (HasFlag(newTransitionParams.TransitionOn, TransitionOn.Visibility))
                {
                    AddVisibilityChangedHandler(target, OnVisibilityChangedForTranslate);
                }
            }
        }

        private static void OnVisibilityChangedForTranslate(object sender, EventArgs e)
        {
            var element = ((FrameworkElement)((PropertyChangeNotifier)sender).PropertySource);
            var visibility = GetVisibility(element);
            if (visibility == Visibility.Visible)
            {
                element.Visibility = Visibility.Visible;
            }
            element.BeginInvoke(new Action(() => DoTranslateTransition(GetTranslate(element), element, null, visibility)), DispatchPriority.DataBind);
        }

        private static void OnDataContextChangedForTranslate(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            element.BeginInvoke(new Action(() => DoTranslateTransition(GetTranslate(element), element, null, null)), DispatchPriority.DataBind);
        }

        private static void OnLoadedForTranslate(object sender, RoutedEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            element.BeginInvoke(new Action(() => DoTranslateTransition(GetTranslate(element), element, OnLoadedForTranslate, null)), DispatchPriority.DataBind);
        }

        private static void DoTranslateTransition(ITranslateParams transitionParams, FrameworkElement target, RoutedEventHandler onLoaded, Visibility? visibility)
        {
            if (onLoaded != null && HasFlag(transitionParams.TransitionOn, TransitionOn.Once))
            {
                target.Loaded -= onLoaded;
            }
            var reverse = IsVisibilityHidden(visibility);

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

            if (visibility.HasValue)
                x.Completed += (_, __) => target.Visibility = visibility.Value;

            x.SetDesiredFrameRate(24);
            y.SetDesiredFrameRate(24);

            (target.RenderTransform).BeginAnimation(TranslateTransform.XProperty, x);
            (target.RenderTransform).BeginAnimation(TranslateTransform.YProperty, y);
        }
    }
}