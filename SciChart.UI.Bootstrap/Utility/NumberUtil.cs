using System;

namespace SciChart.UI.Bootstrap.Utility
{
    public class NumberUtil
    {
        public static int Constrain(int value, int min, int max)
        {
            return Math.Max(Math.Min(value, max), min);
        }
    }
}
