using System.Linq;
using System.Reflection;
using NUnit.Framework;
using SciChart.UI.Bootstrap;

namespace SciChart.UI.Reactive.Tests.Bootstrap
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
