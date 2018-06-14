using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SciChart.Wpf.UI.Bootstrap;

namespace SciChart.Wpf.UI.Reactive.Tests.Stubs
{
    public class StubAssemblyDiscovery : IAssemblyDiscovery
    {
        private IEnumerable<Assembly> _assemblies;

        public StubAssemblyDiscovery(params Assembly[] assemblies)
        {
            _assemblies = assemblies.ToArray();
        }

        public StubAssemblyDiscovery(params string[] assemblyNames)
        {
            _assemblies = assemblyNames.Select(Assembly.Load).ToArray();
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return _assemblies;
        }
    }
}
