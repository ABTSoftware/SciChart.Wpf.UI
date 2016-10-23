namespace SciChart.Wpf.UI.Controls.Converters
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