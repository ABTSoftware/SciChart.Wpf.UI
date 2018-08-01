using System;
using System.Reactive.Subjects;

namespace SciChart.UI.Reactive.Observability
{
    /// <summary>
    /// Emulates a property getter / setter with observability
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyClassIWantToObserveSomethingOn
    /// {
    ///     public ObservableProperty&gt;string&lt; Test { get; }
    /// }
    /// 
    /// var foo = new MyClassIWantToObserveSomethingOn();
    /// foo.Test.Subscribe(s => Console.WriteLine(s));
    /// foo.Test.SetValue("Hello World!");
    /// </code>
    /// </example>
    /// <typeparam name="T"></typeparam>
    public class ObservableProperty<T> : IObservable<T>, IDisposable
    {
        readonly ReplaySubject<T> _items = new ReplaySubject<T>(1);
        private T _current;

        public void SetValue(T value)
        {
            _current = value;
            _items.OnNext(value);
        }

        public T GetValue()
        {
            return _current;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _items.Subscribe(observer);
        }

        public void Dispose()
        {
            _items?.Dispose();
        }
    }
}