using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;
using SciChart.UI.Reactive.Annotations;

namespace SciChart.UI.Reactive.Observability
{
    /// <summary>
    /// An ViewModel class which implements <see cref="INotifyPropertyChanged"/> as well but does not expose observable subject and does not have a Finalizer. 
    /// 
    /// Used for smaller, more frequently created ViewModels where you do not want to add to memory pressure
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        private readonly IDictionary<string, object> _dynamicProperties = new Dictionary<string, object>();

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
    }
}