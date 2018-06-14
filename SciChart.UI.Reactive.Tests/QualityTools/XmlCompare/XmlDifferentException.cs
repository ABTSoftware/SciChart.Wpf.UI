using System;

namespace SciChart.Wpf.UI.Reactive.Tests.QualityTools.XmlCompare
{
    public class XmlDifferentException : Exception
    {
        public XmlDifferentException(string message)
            : base("Xml documents were different: " + message)
        { }
    }
}