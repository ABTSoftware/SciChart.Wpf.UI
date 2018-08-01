using System;
using System.Collections.Generic;
using SciChart.UI.Bootstrap.Utility;

namespace SciChart.UI.Bootstrap
{
    public class AttributedTypeDiscoveryService : IAttributedTypeDiscoveryService
    {
        private static ILogFacade Log = LogManagerFacade.GetLogger(typeof(AttributedTypeDiscoveryService));
        private static readonly IDictionary<Type, IEnumerable<Type>> CachedTypesByAttributeType = new Dictionary<Type, IEnumerable<Type>>();
        private readonly IAssemblyDiscovery _assemblyDiscovery;

        public AttributedTypeDiscoveryService(IAssemblyDiscovery assemblyDiscovery)
        {
            _assemblyDiscovery = assemblyDiscovery;
        }

        public IEnumerable<Type> DiscoverAttributedTypes<T>() where T : Attribute
        {
            var attributeType = typeof(T);

            if (CachedTypesByAttributeType.ContainsKey(attributeType))
                return CachedTypesByAttributeType[attributeType];

            Log.InfoFormat("Discovering Types with Attribute {0}", attributeType);

            var allTypes = new List<Type>();

            foreach (var assembly in _assemblyDiscovery.GetAssemblies())
            {
                allTypes.AddRange(ReflectionUtil.DiscoverTypesWithAttribute(attributeType, assembly));
            }

            var attributedTypes = allTypes.ToArray();
            CachedTypesByAttributeType.Add(attributeType, attributedTypes);
            return attributedTypes;
        }
    }
}