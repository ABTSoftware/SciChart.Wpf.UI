using System.Collections.Generic;
using System.Reflection;

namespace SciChart.UI.Bootstrap
{
    public interface IAssemblyDiscovery
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}