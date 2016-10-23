#region Header
// --------------------------------------------------------------------
// Project:           Transitionz - WPF Animation extensions
// Description:       Collection of markup extensions allowing WPF animations to be applied to elements
// Copyright:		  Copyright © 2013 SciChart Ltd & Transitionz Contributors
// --------------------------------------------------------------------
// Microsoft Public License (Ms-PL) http://opensource.org/licenses/MS-PL
// 
// This license governs use of the accompanying software. If you use the software, you
// accept this license. If you do not accept the license, do not use the software.
// 
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and "distribution" have the
// same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to the software.
// A "contributor" is any person that distributes its contribution under this license.
// "Licensed patents" are a contributor's patent claims that read directly on its contribution.
// 
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
// each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, 
// prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
// each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, 
// sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
// 
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from 
// such contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present 
// in the software.
// (D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of 
// this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a 
// license that complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. 
// You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws,
// the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
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
#if !SILVERLIGHT
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
            var transtionParams = e.NewValue as MarginParamsExtension;
            var target = d as FrameworkElement;
            if (transtionParams == null || target == null)
                return;
            
            target.Margin = transtionParams.From;
            RoutedEventHandler onLoaded = null;
            onLoaded = (_, __) => target.BeginInvoke(() =>
            {
                target.Loaded -= onLoaded;
                var a = new ThicknessAnimation
                {
                    From = transtionParams.From,
                    To = transtionParams.To,
                    FillBehavior = transtionParams.FillBehavior,
                    BeginTime = TimeSpan.FromMilliseconds(transtionParams.BeginTime),
                    Duration = new Duration(TimeSpan.FromMilliseconds(transtionParams.Duration)),
                    EasingFunction = transtionParams.Ease
                };
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
            var scaleParams = e.NewValue as IScaleParams;
            var target = d as FrameworkElement;
            if (scaleParams == null || target == null)
                return;

            var scaleTransform = new ScaleTransform() { ScaleX = scaleParams.From.X, ScaleY = scaleParams.From.Y };
            target.LayoutTransform = scaleTransform;

            RoutedEventHandler onLoaded = null;
            onLoaded = (_, __) => target.BeginInvoke(() =>
            {
                target.Loaded -= onLoaded;

                if (Math.Abs(scaleParams.From.X - scaleParams.To.X) > 0.001)
                {
                    var x = new DoubleAnimation
                    {
                        From = scaleParams.From.X,
                        To = scaleParams.To.X,
                        FillBehavior = scaleParams.FillBehavior,
                        BeginTime = TimeSpan.FromMilliseconds(scaleParams.BeginTime),
                        Duration = new Duration(TimeSpan.FromMilliseconds(scaleParams.Duration)),
                        EasingFunction = scaleParams.Ease,
                        AutoReverse = scaleParams.AutoReverse,
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, x);
                }

                if (Math.Abs(scaleParams.From.Y - scaleParams.To.Y) > 0.001)
                {
                    var y = new DoubleAnimation
                    {
                        From = scaleParams.From.Y,
                        To = scaleParams.To.Y,
                        FillBehavior = scaleParams.FillBehavior,
                        BeginTime = TimeSpan.FromMilliseconds(scaleParams.BeginTime),
                        Duration = new Duration(TimeSpan.FromMilliseconds(scaleParams.Duration)),
                        EasingFunction = scaleParams.Ease,
                        AutoReverse = scaleParams.AutoReverse,
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, y);
                }

            }, DispatchPriority.DataBind);

            if (target.IsLoaded())
                onLoaded(null, null);
            else
                target.Loaded += onLoaded;
        }                
#endif        
    }
}
