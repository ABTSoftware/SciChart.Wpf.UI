using System;

namespace SciChart.UI.Reactive.Tests.QualityTools.XmlCompare
{
    public class XmlDifferentException : Exception
    {
        public XmlDifferentException(string message)
            : base("Xml documents were different: " + message)
        { }
    }
}