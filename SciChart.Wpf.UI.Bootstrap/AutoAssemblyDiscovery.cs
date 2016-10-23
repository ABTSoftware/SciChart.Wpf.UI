using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SciChart.Wpf.UI.Bootstrap.Utility;

namespace SciChart.Wpf.UI.Bootstrap
{    
    public class AutoAssemblyDiscovery : IAssemblyDiscovery
    {
        private static ILogFacade Log = LogManagerFacade.GetLogger(typeof(AutoAssemblyDiscovery));

        private static List<Assembly> _assemblies;
        private readonly List<string> _excludeList = new List<string>(new string[] 
            {
                "Microsoft", 
                "log4net", 
                "System.", 
                "WebApiThrottle",
                "libsodium"
            });

        public AutoAssemblyDiscovery(params string[] asmsToExclude)
        {
            if (asmsToExclude != null)
            {
                _excludeList.AddRange(asmsToExclude);
            }
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            if (_assemblies == null)
            {
                _assemblies = new List<Assembly>();
                var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // Search the directory for assemblies. Exclude those in the exclude list
                var assemblyDlls = Directory.GetFiles(assemblyPath, "*.dll")
                                            .Where(asm => !_excludeList.Any(exl => asm.Contains(exl)));

                var exes = Directory.GetFiles(assemblyPath, "*.exe")
                                    .Where(asm => !_excludeList.Any(exl => asm.Contains(exl)));

                var assemblyFiles = assemblyDlls.Concat(exes).ToArray();

                foreach (var assemblyFile in assemblyFiles)
                {
                    try
                    {
                        Log.InfoFormat(" ... Loading assembly {0}", assemblyFile);
                        var assembly = Assembly.Load(Path.GetFileNameWithoutExtension(assemblyFile));

                        if (!_assemblies.Contains(assembly))
                            _assemblies.Add(assembly);
                    }
                    catch (Exception caught)
                    {
                        Log.Error(caught);
                    }
                }
            }

            return _assemblies;
        }
    }
}