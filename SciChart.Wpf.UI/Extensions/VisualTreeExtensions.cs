using System;
using System.Windows;
using System.Windows.Media;

namespace SciChart.Wpf.UI.Extensions
{
    public static class VisualTreeExtensions
    {
        public static T TryFindChild<T>(this DependencyObject root, Func<T, bool> predicate = null)
         where T : DependencyObject
        {
            if (root == null)
            {
                return null;
            }
            var candidate = root as T;
            if (candidate != null && (predicate == null || predicate(candidate)))
            {
                return candidate;
            }
            var n = VisualTreeHelper.GetChildrenCount(root);
            for (var i = 0; i < n; ++i)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                var foundInChild = child.TryFindChild(predicate);
                if (foundInChild != null)
                {
                    return foundInChild;
                }
            }
            return null;
        }

        public static T TryFindAncestorOrSelf<T>(this DependencyObject child) where T : class
        {
            while (child != null)
            {
                var parent = child as T;
                if (parent != null)
                    return parent;

                child = TryFindParent(child);
            }
            return null;
        }

        public static DependencyObject TryFindParent(this DependencyObject child)
        {
            if (child == null)
                return null;

            //handle content elements separately
            var ce = child as ContentElement;
            if (ce != null)
            {
                var parent = ContentOperations.GetParent(ce);
                if (parent != null)
                    return parent;

                var fce = ce as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            var fe = child as FrameworkElement;
            if (fe != null)
            {
                DependencyObject parent = fe.Parent;
                if (parent != null)
                    return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }
    }

}
