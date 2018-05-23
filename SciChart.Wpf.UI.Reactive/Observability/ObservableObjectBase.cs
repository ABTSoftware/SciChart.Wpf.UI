using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;
using SciChart.Wpf.UI.Reactive.Annotations;

namespace SciChart.Wpf.UI.Reactive.Observability
{
    /// <summary>
    /// An ObservableObject implements <see cref="INotifyPropertyChanged"/> as well as exposes a PropertyChanged <see cref="Subject{T}"/>
    /// which allows observing properties via the <see cref="ObservableObjectExtensions.WhenPropertyChanged{TViewModel, TProperty}"/> extension method. 
    /// 
    /// <code>
    /// ObservableObjectBase o;
    /// o.WhenPropertyChanged(x => x.SomeProperty).Subscribe(...);
    /// </code>
    /// </summary>
    public class ObservableObjectBase : INotifyPropertyChanged, ICompositeDisposable
    {
        private readonly IDictionary<string, object> _dynamicProperties = new Dictionary<string, object>();
        private readonly CompositeDisposable _composite = new CompositeDisposable();

        internal readonly Subject<Tuple<string, object>> PropertyChangedSubject = new Subject<Tuple<string, object>>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the dispatcher synchronization context.
        /// </summary>
        public static SynchronizationContext DispatcherSynchronizationContext { get; set; }

        /// <summary>
        /// Called when property changes with value. Raises the <see cref="INotifyPropertyChanged"/> event as well as publishing OnNext for the reactive <see cref="Subject{T}"/>
        /// for subscribers who have used the <see cref="ObservableObjectExtensions.WhenPropertyChanged{TViewModel, TProperty}"/> extension method
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(object value, [CallerMemberName] string propertyName = null)
        {
            Action notifyPropChanged = () =>
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }   
            };

            if (DispatcherSynchronizationContext != null &&
                DispatcherSynchronizationContext != SynchronizationContext.Current)
            {
                DispatcherSynchronizationContext.Send(_ => notifyPropChanged(), null);
            }
            else
            {
                notifyPropChanged();
            }                   

            PropertyChangedSubject.OnNext(Tuple.Create(propertyName, value));
        }

        /// <summary>
        /// SetValue implementation which raises both INotifyPropertyChanged and reactive OnNext for subscribers who have used the <see cref="ObservableObjectExtensions.WhenPropertyChanged{TViewModel, TProperty}"/> extension method
        /// </summary>
        /// <typeparam name="T">The type of property</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public void SetDynamicValue<T>(T value, [CallerMemberName]string propertyName = null)
        {
            bool isSame = Equals(value, GetDynamicValue<T>(propertyName));
            if (isSame) return;

            _dynamicProperties[propertyName] = value;
            OnPropertyChanged(value, propertyName);
        }

        /// <summary>
        /// GetValue implementation which pairs with <see cref="SetDynamicValue{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of property</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The value.</returns>
        public T GetDynamicValue<T>([CallerMemberName]string propertyName = null)
        {
            object value;
            if (_dynamicProperties.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _composite.Dispose();
            _dynamicProperties.Clear();
        }

        /// <summary>
        /// Adds the disposable to the inner <see cref="CompositeDisposable"/>
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        public void AddDisposable(IDisposable disposable)
        {
            _composite.Add(disposable);
        }
    }
}
