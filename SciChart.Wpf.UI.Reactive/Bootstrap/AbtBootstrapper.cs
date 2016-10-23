using System.Linq;
using System.Reflection;
using SciChart.Wpf.UI.Reactive.Utility;
using Microsoft.Practices.Unity;

namespace SciChart.Wpf.UI.Reactive.Bootstrap
{
    public class ServiceLocator
    {
        private static readonly IUnityContainer _container = new UnityContainer();

        public static IUnityContainer Container { get { return _container; } }
    }

    public class AbtBootstrapper
    {
        private static readonly ILogFacade Log = LogManagerFacade.GetLogger(typeof(AbtBootstrapper));

        private readonly IUnityContainer _container;
        private readonly IAttributedTypeDiscoveryService _attributedTypeDiscovery;

        public AbtBootstrapper(IUnityContainer container, IAttributedTypeDiscoveryService attributedTypeDiscovery)
        {
            _container = container;
            _attributedTypeDiscovery = attributedTypeDiscovery;
            ViewContext.Container = _container;
        }

        public IUnityContainer Container { get { return _container; }}

        public virtual void Initialize()
        {
            var exportTypes = _attributedTypeDiscovery.DiscoverAttributedTypes<ExportTypeAttribute>();

            foreach (var tTo in exportTypes)
            {
                foreach (var exportAttribute in tTo.GetCustomAttributes(true).OfType<ExportTypeAttribute>())
                {
                    if (exportAttribute.CreateAs == CreateAs.Singleton)
                    {
                        Log.DebugFormat("Registering Singleton: {0} as {1}", tTo.Name, exportAttribute.TFrom.Name);
                        _container.RegisterType(exportAttribute.TFrom, tTo, new ContainerControlledLifetimeManager());
                    }
                    else
                    {
                        Log.DebugFormat("Registering: {0} as {1}", tTo.Name, exportAttribute.TFrom.Name);
                        _container.RegisterType(exportAttribute.TFrom, tTo);
                    }
                }
            }
        }        
    }
}
