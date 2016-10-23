using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using SciChart.Wpf.UI.Reactive.Annotations;

namespace SciChart.Wpf.UI.Reactive.Observability
{    
    public class ObservableObjectBase : INotifyPropertyChanged, ICompositeDisposable
    {
        private readonly IDictionary<string, object> _dynamicProperties = new Dictionary<string, object>();
        private readonly CompositeDisposable _composite = new CompositeDisposable();

        internal readonly Subject<Tuple<string, object>> PropertyChangedSubject = new Subject<Tuple<string, object>>();

        public event PropertyChangedEventHandler PropertyChanged;

        public static SynchronizationContext DispatcherSynchronizationContext { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName, object value)
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

        public void SetDynamicValue<T>(string propertyName, T value)
        {
            bool isSame = Equals(value, GetDynamicValue<T>(propertyName));
            if (isSame) return;

            _dynamicProperties[propertyName] = value;
            OnPropertyChanged(propertyName, value);
        }

        public T GetDynamicValue<T>(string propertyName)
        {
            object value;
            if (_dynamicProperties.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        public void Dispose()
        {
            _composite.Dispose();
            _dynamicProperties.Clear();
        }

        public void AddDisposable(IDisposable disposable)
        {
            _composite.Add(disposable);
        }
    }
}
