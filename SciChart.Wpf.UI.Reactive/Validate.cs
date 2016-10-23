using System;
using System.Collections.Generic;
using SciChart.Wpf.UI.Reactive.Extensions;

namespace SciChart.Wpf.UI.Reactive
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
    }
}
