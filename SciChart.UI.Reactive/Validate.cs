using System;
using System.Collections.Generic;
using SciChart.UI.Reactive.Extensions;

namespace SciChart.UI.Reactive
{
    public static class Validate
    {
        public static void NotNull(object argument, string argName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argName);
            }
        }

        public static void NotNullOrEmpty(string argument, string argName)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(argName);
            }
        }

        public static void NotNullOrEmpty<T>(IEnumerable<T> list, string argName)
        {
            if (list.IsNullOrEmpty())
            {
                throw new ArgumentNullException(argName);
            }
        }

        public static void IsTrue(bool expression, string name, string message = null)
        {
            if (expression) return;

            throw new ArgumentException(string.IsNullOrWhiteSpace(message) ? $"The value passed for '{name}' is not valid." : message);
        }
    }
}
