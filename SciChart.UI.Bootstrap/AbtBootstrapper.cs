using System;
using System.Linq;
using System.Reflection;
using Unity;
using Unity.Lifetime;
using SciChart.Wpf.UI.Bootstrap.Utility;

namespace SciChart.Wpf.UI.Bootstrap
{    
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
            this.Initialize(DataMode.Any);
        }

        public virtual void Initialize(DataMode dataMode = DataMode.Any)
        {
            try
            {
                var exportTypes = _attributedTypeDiscovery.DiscoverAttributedTypes<ExportTypeAttribute>();

                foreach (var tTo in exportTypes)
                {
                    var exportAttributes = tTo.GetCustomAttributes(true).OfType<ExportTypeAttribute>();
                    bool preRegistered = false;
                    if (exportAttributes.Count() > 1 && exportAttributes.All(e => e.CreateAs == CreateAs.Singleton))
                    {
                        Log.DebugFormat("Registering Singleton: {0} in preparation for multiple mapping", tTo.Name);
                        _container.RegisterType(tTo, new ContainerControlledLifetimeManager());
                        preRegistered = true;
                    }
                    foreach (var exportAttribute in exportAttributes)
                    {
                        if (dataMode == DataMode.Any || exportAttribute.DataMode == DataMode.Any || exportAttribute.DataMode == dataMode)
                        {
                            if (exportAttribute.CreateAs == CreateAs.Singleton && !preRegistered)
                            {
                                if (string.IsNullOrEmpty(exportAttribute.Name))
                                {
                                    Log.DebugFormat("Registering Singleton: {0} as {1}", tTo.Name, exportAttribute.TFrom.Name);
                                    _container.RegisterType(exportAttribute.TFrom, tTo, new ContainerControlledLifetimeManager());
                                }
                                else
                                {
                                    Log.DebugFormat("Registering Singleton: {0} as {1} with name {2}", tTo.Name, exportAttribute.TFrom.Name, exportAttribute.Name);
                                    _container.RegisterType(exportAttribute.TFrom, tTo, exportAttribute.Name, new ContainerControlledLifetimeManager());
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(exportAttribute.Name))
                                {
                                    Log.DebugFormat("Registering: {0} as {1}", tTo.Name, exportAttribute.TFrom.Name);
                                    _container.RegisterType(exportAttribute.TFrom, tTo);
                                }
                                else
                                {
                                    Log.DebugFormat("Registering: {0} as {1} with name {2}", tTo.Name, exportAttribute.TFrom.Name, exportAttribute.Name);
                                    _container.RegisterType(exportAttribute.TFrom, tTo, exportAttribute.Name);
                                }
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var lex in ex.LoaderExceptions)
                {
                    Log.Error(lex);
                }
                Log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }        
    }
}
