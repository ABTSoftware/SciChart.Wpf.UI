#region Header
// --------------------------------------------------------------------
// Project:           Transitionz - WPF Animation extensions
// Description:       Collection of markup extensions allowing WPF animations to be applied to elements
// Copyright:		  Copyright © 2013-2016 SciChart Ltd & Transitionz Contributors
// License:           Apache v2.0 License https://www.apache.org/licenses/LICENSE-2.0
// --------------------------------------------------------------------
#endregion

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SciChart.Wpf.UI.Transitionz
{
    [TemplatePart(Name = "PART_Background", Type = typeof(System.Windows.Shapes.Rectangle))]
    public class BlinkControl : ContentControl
    {
        public static readonly DependencyProperty FromOpacityProperty = DependencyProperty.Register("FromOpacity", typeof(double), typeof(BlinkControl), new PropertyMetadata(default(double), OnAnyPropertyChanged));
        public static readonly DependencyProperty ToOpacityProperty = DependencyProperty.Register("ToOpacity", typeof(double), typeof(BlinkControl), new PropertyMetadata(default(double), OnAnyPropertyChanged));
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(double), typeof(BlinkControl), new PropertyMetadata(default(double), OnAnyPropertyChanged));
        public static readonly DependencyProperty EasingProperty = DependencyProperty.Register("Easing", typeof(EasingFunctionBase), typeof(BlinkControl), new PropertyMetadata(default(EasingFunctionBase), OnAnyPropertyChanged));
        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register("RadiusX", typeof(double), typeof(BlinkControl), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register("RadiusY", typeof(double), typeof(BlinkControl), new PropertyMetadata(default(double)));        

        private System.Windows.Shapes.Rectangle backgroundElement;

        public BlinkControl()
        {
            DefaultStyleKey = typeof(BlinkControl);
        }

        public double ToOpacity
        {
            get { return (double)GetValue(ToOpacityProperty); }
            set { SetValue(ToOpacityProperty, value); }
        }

        public double FromOpacity
        {
            get { return (double)GetValue(FromOpacityProperty); }
            set { SetValue(FromOpacityProperty, value); }
        }

        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public EasingFunctionBase Easing
        {
            get { return (EasingFunctionBase)GetValue(EasingProperty); }
            set { SetValue(EasingProperty, value); }
        }

        public double RadiusX
        {
            get { return (double)GetValue(RadiusXProperty); }
            set { SetValue(RadiusXProperty, value); }
        }

        public double RadiusY
        {
            get { return (double)GetValue(RadiusYProperty); }
            set { SetValue(RadiusYProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            backgroundElement = (System.Windows.Shapes.Rectangle)this.GetTemplateChild("PART_Background");

            this.UpdateTransition();
        }

        private static void OnAnyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BlinkControl)d).UpdateTransition();
        }        

        private void UpdateTransition()
        {
            if (backgroundElement == null) return;
            
            Transitionz.SetOpacity(backgroundElement, new OpacityParams()
            {
                BeginTime = 0, 
                Duration = Duration,
                Ease = Easing, 
                From = FromOpacity, 
                To = ToOpacity,                
                TransitionOn = TransitionOn.DataContextChanged | TransitionOn.Once
            });
        }
    }
}
