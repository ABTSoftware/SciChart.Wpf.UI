using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SciChart.Wpf.UI.Controls.AttachedBehaviours
{
    internal sealed class PropertyChangeNotifier : DependencyObject, IDisposable
    {
        public static readonly DependencyProperty NotifiersProperty = DependencyProperty.RegisterAttached("Notifiers", typeof (List<PropertyChangeNotifier>), typeof (PropertyChangeNotifier), new PropertyMetadata(default(List<PropertyChangeNotifier>)));

        public static void SetNotifiers(DependencyObject element, List<PropertyChangeNotifier> value)
        {
            element.SetValue(NotifiersProperty, value);
        }

        public static List<PropertyChangeNotifier> GetNotifiers(DependencyObject element)
        {
            return (List<PropertyChangeNotifier>) element.GetValue(NotifiersProperty);
        }

        internal static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(PropertyChangeNotifier), new PropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged)));

        internal event EventHandler ValueChanged;

        private readonly WeakReference _propertySource;

        internal PropertyChangeNotifier(DependencyObject propertySource, string path)
            : this(propertySource, new PropertyPath(path))
        {
        }

        internal PropertyChangeNotifier(DependencyObject propertySource, DependencyProperty property)
            : this(propertySource, new PropertyPath(property))
        {
        }

        internal PropertyChangeNotifier(DependencyObject propertySource, PropertyPath property)
        {
            if (propertySource == null) throw new ArgumentNullException("propertySource");
            
            _propertySource = new WeakReference(propertySource);
            var binding = new Binding { Path = property, Mode = BindingMode.OneWay, Source = propertySource };
            BindingOperations.SetBinding(this, ValueProperty, binding);

            AddNotifier(propertySource, this);
        }

        internal void AddNotifier(DependencyObject propertySource, PropertyChangeNotifier propertyChangeNotifier)
        {
            var notifiers = GetNotifiers(propertySource);
            if (notifiers == null)
            {
                notifiers = new List<PropertyChangeNotifier>();
                SetNotifiers(propertySource, notifiers);
            }

            notifiers.Add(propertyChangeNotifier);
        }        

        internal DependencyObject PropertySource
        {
            get
            {
                try
                {
                    return this._propertySource.IsAlive ? this._propertySource.Target as DependencyObject : null;
                }
                catch
                {
                    return null;
                }
            }
        }

        [Bindable(true)]
        internal object Value
        {
            get { return GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        public void Dispose()
        {
            this.ClearValue(ValueProperty);
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var notifier = (PropertyChangeNotifier)d;
            if (null != notifier.ValueChanged)
                notifier.ValueChanged(notifier, EventArgs.Empty);
        }
    }
}
