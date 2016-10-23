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
            var oldBlurParams = e.OldValue as IBlurParams;
            var newBlurParams = e.NewValue as IBlurParams;
            var target = d as FrameworkElement;
            if (target == null)
                return;

            if (oldBlurParams != null)
            {
                target.Loaded -= Transitionz.OnLoadedForTranslate;
                target.DataContextChanged -= Transitionz.OnDataContextChangedForTranslate;
            }

            if (newBlurParams != null)
            {                
                var blurEffect = new BlurEffect() { Radius = newBlurParams.From };
                target.Effect = blurEffect;

                target.Loaded += OnLoadedForBlur;
                if (target.IsLoaded()) OnLoadedForBlur(target, null);
            }
        }

        private static void OnLoadedForBlur(object sender, RoutedEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            element.BeginInvoke(() => DoBlurTansition(GetBlur(element), element, OnLoadedForBlur), DispatchPriority.DataBind);
        }

        private static void DoBlurTansition(IBlurParams blurParams, FrameworkElement target, RoutedEventHandler onLoaded)
        {
            if (onLoaded != null && Transitionz.HasFlag(blurParams.TransitionOn, TransitionOn.Once))
            {
                target.Loaded -= onLoaded;
            }

            var reverse = false;

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

            if (blurParams.To == 0.0)
            {
                a.Completed += (_, __) => target.Effect = null;
            }
            a.SetDesiredFrameRate(24);

            var storyboard = new Storyboard();
            storyboard.Children.Add(a);
            Storyboard.SetTarget(a, ((BlurEffect)target.Effect));
            Storyboard.SetTargetProperty(a, new PropertyPath(BlurEffect.RadiusProperty));
            FreezeHelper.SetFreeze(storyboard, true);
            storyboard.Begin();
        }
    }
}