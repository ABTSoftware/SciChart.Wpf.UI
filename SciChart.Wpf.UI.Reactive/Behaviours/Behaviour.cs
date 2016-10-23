using System;
using System.Reactive.Disposables;
using SciChart.Wpf.UI.Reactive.Observability;

namespace SciChart.Wpf.UI.Reactive.Behaviours
{
    public interface IBehaviour : ICompositeDisposable
    {        
    }

    /// <summary>
    /// A Behaviour is a place to put a unit of work on an <see cref="ObservableObjectBase"/>, for instance, handling the observability
    /// of search and updating of results.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="ObservableObjectBase"/> we are targetting</typeparam>
    public class Behaviour<T> : IBehaviour where T:ObservableObjectBase
    {
        private readonly T _target;
        private readonly CompositeDisposable _composite = new CompositeDisposable();

        public Behaviour(T target)
        {
            _target = target;
        }

        public T Target
        {
            get { return _target; }
        }

        public virtual void Dispose()
        {
            _composite.Dispose();
        }

        public void AddDisposable(IDisposable disposable)
        {
            _composite.Add(disposable);
        }
    }
}