using System;
using System.Reactive.Disposables;
using SciChart.UI.Bootstrap;
using SciChart.UI.Reactive;
using SciChart.UI.Reactive.Observability;

namespace SciChart.UI.Reactive.Traits
{
    public interface IViewModelTrait : ICompositeDisposable
    {        
    }

    /// <summary>
    /// A ViewModelTrait is a place to put a unit of work on an <see cref="ObservableObjectBase"/>, for instance, handling the observability
    /// of search and updating of results.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="ObservableObjectBase"/> we are targetting</typeparam>
    public class ViewModelTrait<T> : FinalizableObject, IViewModelTrait where T:ObservableObjectBase
    {
        private readonly T _target;
        private readonly CompositeDisposable _composite = new CompositeDisposable();

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelTrait{T}"/> class.
        /// </summary>
        /// <param name="target">The target <see cref="ObservableObjectBase"/>.</param>
            public ViewModelTrait(T target)
        {
            _target = target;
        }

        public T Target
        {
            get { return _target; }
        }

        /// <summary>
        /// Adds the disposable to the inner  <see cref="CompositeDisposable"/>
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        public void AddDisposable(IDisposable disposable)
        {
            _composite.Add(disposable);
        }

        protected override void Dispose(bool disposing)
        {
            _composite.Dispose();
            _composite.Clear();
        }
    }
}