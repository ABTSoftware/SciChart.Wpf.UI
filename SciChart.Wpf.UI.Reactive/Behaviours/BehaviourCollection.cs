using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using SciChart.Wpf.UI.Reactive.Observability;
using Microsoft.Practices.Unity;

namespace SciChart.Wpf.UI.Reactive.Behaviours
{
    public class BehaviourCollection: ICompositeDisposable
    {
        private readonly ObservableObjectBase _parent;
        private readonly IUnityContainer _container;
        private readonly IDictionary<Type, IBehaviour> _children = new Dictionary<Type, IBehaviour>();
        private readonly CompositeDisposable _composite = new CompositeDisposable();

        public BehaviourCollection(ObservableObjectBase parent, IUnityContainer container)
        {
            _parent = parent;
            this.DisposeWith(parent);
            _container = container;
        }

        public T Add<T>() where T : IBehaviour
        {                        
            IBehaviour oldBehaviour;
            if (_children.TryGetValue(typeof (T), out oldBehaviour))
            {
                oldBehaviour.Dispose();
                _composite.Remove(oldBehaviour);
            }

            var parameterOverride = new ParameterOverrides { { "target", _parent } };
            var newBehaviour = _container.Resolve<T>(parameterOverride);
            _children[typeof (T)] = newBehaviour;
            newBehaviour.DisposeWith(this);

            return newBehaviour;
        }

        public T TryGet<T>()
        {
            IBehaviour behaviour = null;
            _children.TryGetValue(typeof(T), out behaviour);
            return (T) behaviour;
        }

        public bool Contains<T>()
        {
            return _children.ContainsKey(typeof (T));
        }

        public void Dispose()
        {
            _composite.Dispose();
        }

        public void AddDisposable(IDisposable disposable)
        {
            _composite.Add(disposable);
        }
    }
}