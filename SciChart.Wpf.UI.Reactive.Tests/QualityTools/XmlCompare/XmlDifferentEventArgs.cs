using System;

namespace SciChart.Wpf.UI.Reactive.Tests.QualityTools.XmlCompare
{
    public class XmlDifferentEventArgs : EventArgs
    {
        private readonly XmlDifferenceType differenceType;
        private readonly string expectedXPath;
        private readonly string actualXPath;
        private readonly string valueExpected;
        private readonly string valueActual;
        private bool handled = false;

        public XmlDifferentEventArgs(XmlDifferenceType differenceType, string expectedXPath, string actualXPath)
            : this(differenceType, expectedXPath, actualXPath, null, null)
        { }

        public XmlDifferentEventArgs(XmlDifferenceType differenceType, string expectedXPath, string actualXPath, string valueExpected, string valueActual)
        {
            this.differenceType = differenceType;
            this.valueActual = valueActual;
            this.valueExpected = valueExpected;
            this.actualXPath = actualXPath;
            this.expectedXPath = expectedXPath;
        }

        public XmlDifferenceType DifferenceType
        {
            get { return differenceType; }
        }

        public string ExpectedXPath
        {
            get { return expectedXPath; }
        }

        public string ActualXPath
        {
            get { return actualXPath; }
        }

        public bool Handled
        {
            get { return handled; }
            set { handled = value; }
        }

        public string ValueExpected
        {
            get { return valueExpected; }
        }

        public string ValueActual
        {
            get { return valueActual; }
        }

        public override string ToString()
        {
            return
                String.Format(
                    "Difference type: {0}, expected: {1}, actual: {2}\r\nPosition in expected document: {3}\r\nPosition in actual document: {4}", differenceType, valueExpected ?? "", valueActual ?? "", expectedXPath ?? "", actualXPath ?? "");
        }
    }
}