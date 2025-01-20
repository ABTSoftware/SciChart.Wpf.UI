using System;
using System.Collections.Generic;
using Unity;
using Unity.Extension;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Resolution;

namespace SciChart.UI.Reactive.Tests.QualityTools.Stubs
{
    public class StubContainer : IUnityContainer
    {
        public IDictionary<Type, Type> TypeRegistrations = new Dictionary<Type, Type>();
        public IDictionary<Type, Type> SingletonRegistrations = new Dictionary<Type, Type>();
        public IDictionary<Type, object> InstanceRegistrations = new Dictionary<Type, object>();

        public IUnityContainer Parent { get; }
        public IEnumerable<IContainerRegistration> Registrations { get; }

        public IUnityContainer RegisterType(Type registeredType, Type mappedToType, string name, ITypeLifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            if (registeredType == null)
            {
                registeredType = mappedToType;
            }

            if (lifetimeManager is ContainerControlledLifetimeManager || SingletonRegistrations.ContainsKey(mappedToType))
            {
                SingletonRegistrations[registeredType] = mappedToType;
            }
            else
            {
                TypeRegistrations[registeredType] = mappedToType;
            }

            return this;
        }

        public IUnityContainer RegisterInstance(Type type, string name, object instance, IInstanceLifetimeManager lifetimeManager)
        {
            InstanceRegistrations[type] = instance;
            return this;
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return Activator.CreateInstance(t);
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public void Teardown(object o)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            throw new NotImplementedException();
        }

        public object Configure(Type configurationInterface)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RemoveAllExtensions()
        {
            throw new NotImplementedException();
        }

        public IUnityContainer CreateChildContainer()
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type type, string name)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterFactory(Type type, string name, Func<IUnityContainer, Type, string, object> factory, IFactoryLifetimeManager lifetimeManager)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}