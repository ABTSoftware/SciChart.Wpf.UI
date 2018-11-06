using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SciChart.Wpf.UI.Transitionz.AttachedBehaviors;

namespace SciChart.Wpf.UI.Transitionz
{
    public partial class Transitionz
    {               
        public static readonly DependencyProperty LayoutScaleProperty =
            DependencyProperty.RegisterAttached("LayoutScale", typeof(IScaleParams), typeof(UI.Transitionz.Transitionz), new PropertyMetadata(default(IScaleParams), OnLayoutScaleParamsChanged));

        public static void SetLayoutScale(UIElement element, IScaleParams value)
        {
            element.SetValue(LayoutScaleProperty, value);
        }

        public static IScaleParams GetLayoutScale(UIElement element)
        {
            return (IScaleParams)element.GetValue(LayoutScaleProperty);
        }

        private static void OnLayoutScaleParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldTransitionParams = e.OldValue as IScaleParams;
            var newTransitionParams = e.NewValue as IScaleParams;
            var target = d as FrameworkElement;
            if (target == null)
                return;

            if (oldTransitionParams != null)
            {
                target.Loaded -= Transitionz.OnLoadedForScale;
                target.DataContextChanged -= Transitionz.OnDataContextChangedForScale;
            }

            if (newTransitionParams != null)
            {
                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.Loaded))
                {
                    target.Loaded += OnLoadedForScale;
                    if (target.IsLoaded()) OnLoadedForScale(target, null);
                }

                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.DataContextChanged))
                {
                    target.DataContextChanged += OnDataContextChangedForScale;
                }

                if (Transitionz.HasFlag(newTransitionParams.TransitionOn, TransitionOn.Visibility))
                {
                    Transitionz.AddVisibilityChangedHandler(target, OnVisibilityChangedForScale);
                }
            }
        }

        private static void OnVisibilityChangedForScale(object sender, EventArgs e)
        {
            var element = ((FrameworkElement)((PropertyChangeNotifier)sender).PropertySource);
            var visibility = Transitionz.GetVisibility(element);
            if (visibility == Visibility.Visible)
            {
                element.Visibility = Visibility.Visible;
            }
            DoScaleTansition(GetLayoutScale(element), element, null, visibility);
        }

        private static void OnLoadedForScale(object sender, RoutedEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            DoScaleTansition(GetLayoutScale(element), element, OnLoadedForBlur, null);
        }

        private static void OnDataContextChangedForScale(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            DoScaleTansition(GetLayoutScale(element), element, null, null);
        }

        private static void DoScaleTansition(
            IScaleParams scaleParams,
            FrameworkElement target,
            RoutedEventHandler onLoaded,
            Visibility? visibility)
        {
            var reverse = Transitionz.IsVisibilityHidden(visibility);
            var scaleTransform = new ScaleTransform()
			{
			    ScaleX = reverse ? scaleParams.To.X : scaleParams.From.X,
			    ScaleY = reverse ? scaleParams.To.Y : scaleParams.From.Y
			};
			target.LayoutTransform = scaleTransform;

            if (onLoaded != null && Transitionz.HasFlag(scaleParams.TransitionOn, TransitionOn.Once))
            {
                target.Loaded -= onLoaded;
            }            

            if (Math.Abs(scaleParams.From.X - scaleParams.To.X) > 0.001)
			{
				var x = new DoubleAnimation
				{
					From = reverse ? scaleParams.To.X : scaleParams.From.X,
                    To = reverse ? scaleParams.From.X : scaleParams.To.X,
                    FillBehavior = scaleParams.FillBehavior,
					BeginTime = TimeSpan.FromMilliseconds(scaleParams.BeginTime),
					Duration = new Duration(TimeSpan.FromMilliseconds(scaleParams.Duration)),
					EasingFunction = reverse ? (scaleParams.ReverseEase ?? scaleParams.Ease) : scaleParams.Ease,
                    AutoReverse = scaleParams.AutoReverse,
				};

			    x.SetDesiredFrameRate(24);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, x);
			}
            
			if (Math.Abs(scaleParams.From.Y - scaleParams.To.Y) > 0.001)
			{
				var y = new DoubleAnimation
				{
				    From = reverse ? scaleParams.To.Y : scaleParams.From.Y,
				    To = reverse ? scaleParams.From.Y : scaleParams.To.Y,
				    FillBehavior = scaleParams.FillBehavior,
				    BeginTime = TimeSpan.FromMilliseconds(scaleParams.BeginTime),
				    Duration = new Duration(TimeSpan.FromMilliseconds(scaleParams.Duration)),
				    EasingFunction = reverse ? (scaleParams.ReverseEase ?? scaleParams.Ease) : scaleParams.Ease,
				    AutoReverse = scaleParams.AutoReverse,
                };

			    y.SetDesiredFrameRate(24);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, y);
			}
        }
    }
}
