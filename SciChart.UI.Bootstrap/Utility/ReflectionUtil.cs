using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SciChart.UI.Bootstrap.Utility
{
    internal static class ReflectionUtil
    {
        /// <summary>
        /// Discovers all objects in the given assembly that are decorated with an attribute
        /// </summary>
        /// <devdoc>
        /// Type T must be an attribute. This method searches the given package for all classes that
        /// derive from interface type T</devdoc>
        /// <param name="interfaceType">The interface type to search by</param>
        /// <param name="objectAssembly">The assembly to search in</param>
        /// <returns>A list of object types that implement T</returns>
        /// <exception cref="InvalidOperationException"/>
        public static List<Type> DiscoverTypesWithAttribute(Type attributeType, Assembly assembly)
        {
            // Check type is an attribute
            if (!(attributeType.BaseType == typeof(Attribute)))
            {
                throw new InvalidOperationException("The type " + attributeType.GetType().ToString() + " is not an Attribute");
            }

            // get all types from the assembly
            Type[] assemblyTypes = assembly.GetTypes();
            var discoveredTypes = new List<Type>();

            // Loop through types
            foreach (Type objectType in assemblyTypes)
            {
                // Get all attribute types for the current type
                var objectAttributes = objectType.GetCustomAttributes(false).Select(attr => attr.GetType());

                if (objectAttributes.Any(x => x == attributeType || x.IsSubclassOf(attributeType)))
                    discoveredTypes.Add(objectType);
            }

            return discoveredTypes;
        }
    }
}