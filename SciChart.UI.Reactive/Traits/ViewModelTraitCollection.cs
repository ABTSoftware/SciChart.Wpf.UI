using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using SciChart.Wpf.UI.Reactive.Observability;

namespace SciChart.Wpf.UI.Reactive.Traits
{
    /// <summary>
    /// A collection of <see cref="IViewModelTrait"/> - traits or viewmodel behaviours which are added to <see cref="ViewModelWithTraitsBase"/> 
    /// to allow for better segmentation of code and decoupling of dependencies 
    /// </summary>
    public class ViewModelTraitCollection : ICompositeDisposable
    {
        private readonly ObservableObjectBase _parent;
        private readonly ITraitDependencyResolver _resolver;
        private readonly IDictionary<Type, IViewModelTrait> _children = new Dictionary<Type, IViewModelTrait>();
        private readonly CompositeDisposable _composite = new CompositeDisposable();

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelTraitCollection"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="container">The container.</param>
        public ViewModelTraitCollection(ObservableObjectBase parent, ITraitDependencyResolver resolver)
        {
            _parent = parent;

            this.DisposeWith(parent);

            _resolver = resolver;
        }

        /// <summary>
        /// Adds the trait instance <see cref="T"/> to this collection, returning the actual instance 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Add<T>() where T : IViewModelTrait
        {                        
            IViewModelTrait oldBehaviour;
            if (_children.TryGetValue(typeof (T), out oldBehaviour))
            {
                oldBehaviour.Dispose();
                _composite.Remove(oldBehaviour);
            }

            var newBehaviour = _resolver.ResolveWithParent<T>(_parent);
            if (newBehaviour != null)
            {
                _children[typeof(T)] = newBehaviour;
                newBehaviour.DisposeWith(this);
            }

            return newBehaviour;
        }

        /// <summary>
        /// Tries to get a trait of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T TryGet<T>()
        {
            IViewModelTrait behaviour = null;
            _children.TryGetValue(typeof(T), out behaviour);
            return (T) behaviour;
        }

        /// <summary>
        /// Determines whether a trait of type <see cref="T"/> exists in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool Contains<T>()
        {
            return _children.ContainsKey(typeof (T));
        }

        /// <summary>
        /// Disposes this plus any contained objects in the inner <see cref="CompositeDisposable"/>
        /// </summary>
        public void Dispose()
        {
            _composite.Dispose();
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