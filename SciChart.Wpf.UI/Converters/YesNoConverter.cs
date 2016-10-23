namespace SciChart.Wpf.UI.Converters
{
    public class YesNoConverter : BoolToValueConverter
    {
        public YesNoConverter()
        {
            TrueValue = "Yes";
            FalseValue = "No";
        }
    }
}