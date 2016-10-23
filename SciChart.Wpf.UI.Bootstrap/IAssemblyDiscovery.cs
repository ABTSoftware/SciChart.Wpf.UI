using System.Collections.Generic;
using System.Reflection;

namespace SciChart.Wpf.UI.Bootstrap
{
    public interface IAssemblyDiscovery
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}