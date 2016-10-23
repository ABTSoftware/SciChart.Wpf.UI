using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SciChart.Wpf.UI.Reactive.Tests.QualityTools.Stubs;
using SciChart.Wpf.UI.Reactive.Tests.Stubs;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using SciChart.Wpf.UI.Bootstrap;

namespace SciChart.Wpf.UI.Reactive.Tests.Bootstrap
{
    #region Required Classes
    public interface IExportedType
    {        
    }

    [ExportType(typeof(IExportedType))]
    public class ExportedType : IExportedType
    {        
    }

    public interface IExportedSingleton
    {        
    }

    [ExportType(typeof(IExportedSingleton), CreateAs.Singleton)]
    public class ExportedSingleton : IExportedSingleton
    {
    }

    public interface ISomeOtherConfig
    {        
    }

    public interface ISomeConfig : ISomeOtherConfig
    {        
    }

    [ExportType(typeof(ISomeConfig), CreateAs.Singleton)]
    [ExportType(typeof(ISomeOtherConfig), CreateAs.Singleton)]
    public class ExportedConfig : ISomeConfig
    {        
    }
    #endregion

    [TestFixture]
    public class AbtBootstrapperTests
    {        
        [Test]
        public void ShouldRegisterTypesWithContainer()
        {            
            // Arrange
            var container = new StubContainer();
            var asmDiscovery = new StubAssemblyDiscovery(Assembly.GetExecutingAssembly().FullName);
            var attributedDiscovery = new AttributedTypeDiscoveryService(asmDiscovery);
            var bootstrapper = new AbtBootstrapper(container, attributedDiscovery);

            // Act
            bootstrapper.Initialize();

            // Assert
            Assert.That(container.TypeRegistrations.ContainsKey(typeof(IExportedType)));
            Assert.That(container.SingletonRegistrations.ContainsKey(typeof(IExportedSingleton)));
            Assert.That(container.SingletonRegistrations.ContainsKey(typeof(ISomeConfig)));
            Assert.That(container.SingletonRegistrations.ContainsKey(typeof(ISomeOtherConfig)));
        }

        [Test]
        public void ShouldResolveTypesAfterAutoWireup()
        {
            // Arrange
            var container = new UnityContainer();
            var asmDiscovery = new StubAssemblyDiscovery(Assembly.GetExecutingAssembly().FullName);
            var attributedDiscovery = new AttributedTypeDiscoveryService(asmDiscovery);
            var bootstrapper = new AbtBootstrapper(container, attributedDiscovery);

            // Act
            bootstrapper.Initialize();

            var exportedType0 = container.Resolve<IExportedType>();
            var exportedType1 = container.Resolve<IExportedType>();

            var exportedSingleton0 = container.Resolve<IExportedSingleton>();
            var exportedSingleton1 = container.Resolve<IExportedSingleton>();

            var exportedConfig0 = container.Resolve<ISomeConfig>();
            var exportedConfig1 = container.Resolve<ISomeConfig>();
            var exportedConfig2 = container.Resolve<ISomeOtherConfig>();
            var exportedConfig3 = container.Resolve<ISomeOtherConfig>();

            // Assert            
            Assert.That(ReferenceEquals(exportedType0, exportedType1), Is.False);
            Assert.That(ReferenceEquals(exportedSingleton0, exportedSingleton1), Is.True);
            Assert.That(ReferenceEquals(exportedConfig0, exportedConfig1), Is.True);
            Assert.That(ReferenceEquals(exportedConfig2, exportedConfig3), Is.True);
            Assert.That(ReferenceEquals(exportedConfig0, exportedConfig2), Is.True);
        }
    }
}
