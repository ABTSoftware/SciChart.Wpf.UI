namespace SciChart.UI.Reactive.Tests.QualityTools.XmlCompare
{
    public static class CompareXml
    {
        public static void AssertAreEqual(string expected, string actual)
        {
            XmlComparer compare = new XmlComparer(expected);
            compare.Compare(actual);
        }
    }
}