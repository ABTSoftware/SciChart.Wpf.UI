using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SciChart.Wpf.UI.Reactive.Tests.QualityTools
{
    public static class ResourceHelper
    {
        public static string LoadFileFromResource(string resourceName)
        {
            var asm = Assembly.GetCallingAssembly();
            var resource = asm.GetManifestResourceNames().SingleOrDefault(n => n.ToUpper().Contains(resourceName.ToUpper()));

            string resErrorMessage = string.Format("Unable to find an embedded resource named like '{0}' in assembly {1}", resourceName, asm.FullName);
            if (resource == null)
            {
                throw new InvalidOperationException(resErrorMessage);
            }

            using (Stream input = asm.GetManifestResourceStream(resource))
            using (StreamReader reader = new StreamReader(input))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
