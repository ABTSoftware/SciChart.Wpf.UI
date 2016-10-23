using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciChart.Wpf.UI.Reactive.Utility
{
    public class NumberUtil
    {
        public static int Constrain(int value, int min, int max)
        {
            return Math.Max(Math.Min(value, max), min);
        }
    }
}
