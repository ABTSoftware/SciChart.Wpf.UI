using System.Linq;
using System.Reflection;
using SciChart.Wpf.UI.Reactive.Bootstrap;
using NUnit.Framework;

namespace SciChart.Wpf.UI.Reactive.Tests.Bootstrap
{
    [TestFixture]
    public class AssemblyDiscoveryTests
    {
        [Test]
        public void ShouldDiscoverAssemblies()
        {
            var assemblyDiscovery = new AutoAssemblyDiscovery();
            var assemblies = assemblyDiscovery.GetAssemblies();

            Assert.That(assemblies, Is.Not.Empty);
            var assemblyNames = assemblies.Select(x => x.FullName).ToArray();

            Assert.That(assemblyNames, Has.Member(Assembly.GetAssembly(typeof(AutoAssemblyDiscovery)).FullName));
        }
    }
}
