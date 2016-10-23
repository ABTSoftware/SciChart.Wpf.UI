using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Media.Animation
{
    internal static class AnimationExtensions
    {
        internal static void SetDesiredFrameRate(this Timeline animation, int? fps)
        {
#if !SILVERLIGHT
            Timeline.SetDesiredFrameRate(animation, fps);
#endif
        }

#if SILVERLIGHT
        public static void BeginAnimation(this DependencyObject obj, DependencyProperty property, Timeline animation)
        {
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(storyboard, obj);
            Storyboard.SetTargetProperty(storyboard, new PropertyPath(property));
            storyboard.Begin();
        }
#endif
    }
}
