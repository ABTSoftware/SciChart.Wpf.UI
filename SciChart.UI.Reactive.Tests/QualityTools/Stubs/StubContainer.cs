using System;
using System.Collections.Generic;
using Unity;
using Unity.Extension;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

namespace SciChart.Wpf.UI.Reactive.Tests.QualityTools.Stubs
{
    public class StubContainer : IUnityContainer
    {
        public void Dispose()
        {
        }
        
        public IDictionary<Type, Type> TypeRegistrations = new Dictionary<Type, Type>();
        public IDictionary<Type, Type> SingletonRegistrations = new Dictionary<Type, Type>();
        public IDictionary<Type, object> InstanceRegistrations = new Dictionary<Type, object>(); 

        public IUnityContainer RegisterType(Type @from, Type to, string name, LifetimeManager lifetimeManager,
                                            params InjectionMember[] injectionMembers)
        {
            if (from == null)
                from = to;
            if (lifetimeManager is ContainerControlledLifetimeManager)
                SingletonRegistrations[from] = to;
            else if (SingletonRegistrations.ContainsKey(to))
                SingletonRegistrations[from] = to;
            else
                TypeRegistrations[from] = to;

            return this;
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            InstanceRegistrations[t] = instance;
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

        public IUnityContainer Parent { get; private set; }
        public IEnumerable<IContainerRegistration> Registrations { get; private set; }
    }
}

