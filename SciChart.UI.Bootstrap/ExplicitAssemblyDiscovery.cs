using System.Collections.Generic;
using System.Reflection;

namespace SciChart.UI.Bootstrap
{
    public class ExplicitAssemblyDiscovery : IAssemblyDiscovery
    {
        private readonly Assembly[] _assemblies;

        public ExplicitAssemblyDiscovery(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return _assemblies;
        }
    }
}
