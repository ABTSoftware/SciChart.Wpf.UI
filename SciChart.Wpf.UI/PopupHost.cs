using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using SciChart.Wpf.UI.Controls.Extensions;

namespace SciChart.Wpf.UI.Controls
{
    public class PopupHost : ContentControl
    {
        public static readonly DependencyProperty AnimateProperty = DependencyProperty.Register("Animate", typeof(bool), typeof(PopupHost), new PropertyMetadata(true));
        public static readonly DependencyProperty SurroundSizeProperty = DependencyProperty.Register("SurroundSize", typeof(GridLength), typeof(PopupHost), new PropertyMetadata(default(GridLength)));                
        public static readonly DependencyProperty BorderEffectProperty = DependencyProperty.Register("BorderEffect", typeof(Effect), typeof(PopupHost));
        public static readonly DependencyProperty FocusOnVisibleProperty = DependencyProperty.Register("FocusOnVisible", typeof(bool), typeof(PopupHost), new PropertyMetadata(true));

        private IInputElement _prevFocus;

        static PopupHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupHost), new FrameworkPropertyMetadata(typeof(PopupHost)));
            VisibilityProperty.OverrideMetadata(typeof(PopupHost), new FrameworkPropertyMetadata(Visibility.Visible, (s, e) => ((PopupHost)s).OnVisibilityChanged(e)));
        }

        public bool Animate
        {
            get { return (bool)GetValue(AnimateProperty); }
            set { SetValue(AnimateProperty, value); }
        }

        public GridLength SurroundSize
        {
            get { return (GridLength)GetValue(SurroundSizeProperty); }
            set { SetValue(SurroundSizeProperty, value); }
        }

        public Effect BorderEffect
        {
            get { return (Effect)GetValue(BorderEffectProperty); }
            set { SetValue(BorderEffectProperty, value); }
        }

        public bool FocusOnVisible
        {
            get { return (bool)GetValue(FocusOnVisibleProperty); }
            set { SetValue(FocusOnVisibleProperty, value); }
        }

        private void OnVisibilityChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!FocusOnVisible) return;
            if (((Visibility)e.NewValue) == Visibility.Visible)
            {
                _prevFocus = null;
                var window = this.TryFindAncestorOrSelf<Window>();
                if (window != null)
                {
                    _prevFocus = window.TryFindChild<UIElement>(x => x.IsFocused);
                }
                Focus();
                Dispatcher.BeginInvoke(new Action(SetFocus), DispatcherPriority.Background);
            }
            else if (_prevFocus != null)
            {
                _prevFocus.Focus();
            }
        }

        private void SetFocus()
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}


