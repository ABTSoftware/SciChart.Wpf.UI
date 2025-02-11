﻿using SciChart.UI.Bootstrap;
using SciChart.UI.Reactive.Observability;
using Unity;
using Unity.Resolution;

namespace SciChart.UI.Reactive.Traits
{
    public interface ITraitDependencyResolver
    {
        T ResolveWithParent<T>(ObservableObjectBase parent);
    }

    [ExportType(typeof(ITraitDependencyResolver))]
    public class TraitDependencyResolver : ITraitDependencyResolver
    {
        private readonly IUnityContainer _container;

        public TraitDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public T ResolveWithParent<T>(ObservableObjectBase parent)
        {
            var parameterOverride = new ParameterOverride("target", parent);
            return _container.Resolve<T>(parameterOverride);
        }
    }
}