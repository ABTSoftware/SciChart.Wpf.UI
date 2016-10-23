using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SciChart.Wpf.UI.Reactive.Bootstrap
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
