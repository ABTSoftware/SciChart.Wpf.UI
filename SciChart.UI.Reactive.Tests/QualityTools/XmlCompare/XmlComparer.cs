using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SciChart.Wpf.UI.Reactive.Tests.QualityTools.XmlCompare
{
    public class XmlComparer
    {
        private const string XMLNS = "xmlns";
        private readonly XmlDocument expectedDocument;

        public event EventHandler<XmlDifferentEventArgs> DifferenceFound;

        public XmlComparer(string expectedXml)
        {
            if (String.IsNullOrEmpty(expectedXml))
            {
                throw new ArgumentNullException("expectedXml");
            }
            expectedDocument = new XmlDocument();
            expectedDocument.LoadXml(expectedXml);
        }

        public void Compare(string actualXml)
        {
            if (String.IsNullOrEmpty(actualXml))
            {
                throw new XmlDifferentException("Actual xml was null or empty");
            }
            XmlDocument actualDocument = new XmlDocument();
            actualDocument.LoadXml(actualXml);
            CompareElements(expectedDocument.DocumentElement, actualDocument.DocumentElement);
        }

        private void CompareElements(XmlNode expected, XmlNode actual)
        {
            if (expected == null && actual == null)
            {
                return;
            }
            if (expected == null)
            {
                HandleDifference(XmlDifferenceType.ExtraNode, null, GetXPath(actual));
                return;
            }
            if (actual == null)
            {
                HandleDifference(XmlDifferenceType.MissingNode, GetXPath(expected), null);
                return;
            }
            if (expected.Name != actual.Name)
            {
                HandleDifference(XmlDifferenceType.DifferentNode, GetXPath(expected), GetXPath(actual), expected.LocalName, actual.LocalName);
            }
            if (expected.NodeType != actual.NodeType)
            {
                //TODO more details needed?
                HandleDifference(XmlDifferenceType.DifferentNode, GetXPath(expected), GetXPath(actual), expected.NodeType.ToString(), actual.NodeType.ToString());
            }
            if (expected.Value != actual.Value)
            {
                HandleDifference(XmlDifferenceType.NodeValue, GetXPath(expected), GetXPath(actual), expected.Value, actual.Value);
            }

            CompareAttributes(expected, actual);

            //compare children
            for (int i = 0; i < actual.ChildNodes.Count || i < expected.ChildNodes.Count; i++)
            {
                //ChildNodes[i] returns null if the node doesn't exist
                CompareElements(expected.ChildNodes[i], actual.ChildNodes[i]);
            }
        }

        private void CompareAttributes(XmlNode expected, XmlNode actual)
        {
            if (expected.Attributes == actual.Attributes)
            {
                return;
            }
            if (expected.Attributes == null)
            {
                foreach (XmlAttribute actualAttribute in actual.Attributes)
                {
                    HandleDifference(XmlDifferenceType.MissingAttribute, null, GetXPath(actualAttribute));
                }
                return;
            }
            if (actual.Attributes == null)
            {
                foreach (XmlAttribute expectedAttribute in expected.Attributes)
                {
                    HandleDifference(XmlDifferenceType.ExtraAttribute, GetXPath(expectedAttribute), null);
                }
                return;
            }

            foreach (XmlAttribute expectedAttribute in expected.Attributes)
            {
                if (!IsSpecialAttribute(expectedAttribute))
                {
                    XmlAttribute actualAttribute = actual.Attributes[expectedAttribute.Name];
                    if (actualAttribute == null)
                    {
                        HandleDifference(XmlDifferenceType.MissingAttribute, GetXPath(expectedAttribute), null);
                        continue;
                    }
                    if (actualAttribute.Value != expectedAttribute.Value)
                    {
                        HandleDifference(XmlDifferenceType.AttributeValue, GetXPath(expectedAttribute),
                                         GetXPath(actualAttribute), expectedAttribute.Value, actualAttribute.Value);
                    }
                }
            }

            foreach (XmlAttribute actualAttribute in actual.Attributes)
            {
                XmlAttribute expectedAttribute = expected.Attributes[actualAttribute.Name];
                if (expectedAttribute == null && !IsSpecialAttribute(actualAttribute))
                {
                    HandleDifference(XmlDifferenceType.ExtraAttribute, null, GetXPath(actualAttribute));
                }
            }
        }

        private void HandleDifference(XmlDifferenceType differenceType, string expectedXPath, string actualXPath)
        {
            HandleDifference(differenceType, expectedXPath, actualXPath, null, null);
        }

        private void HandleDifference(XmlDifferenceType differenceType, string expectedXPath, string actualXPath, string valueExpected, string valueActual)
        {
            XmlDifferentEventArgs args = new XmlDifferentEventArgs(differenceType, expectedXPath, actualXPath, valueExpected, valueActual);
            if (DifferenceFound != null)
            {
                DifferenceFound(this, args);
            }
            if (!args.Handled)
            {
                throw new XmlDifferentException(args.ToString());
            }
        }

        private static string GetXPath(XmlNode node)
        {
            StringBuilder xPathSB = new StringBuilder();
            if (node == null)
            {
                return null;
            }
            XmlAttribute attribute = node as XmlAttribute;
            if (attribute != null)
            {
                GetXPathForNodeRecursive(attribute.OwnerElement, xPathSB);
                xPathSB.AppendFormat("/@{0}", attribute.Name);
            }
            else if (node.NodeType == XmlNodeType.Text)
            {
                GetXPathForNodeRecursive(node.ParentNode, xPathSB);
                xPathSB.Append("/text()");
            }
            else
            {
                GetXPathForNodeRecursive(node, xPathSB);
            }
            return xPathSB.ToString();
        }

        private static void GetXPathForNodeRecursive(XmlNode node, StringBuilder xPathSB)
        {
            int number = 1;
            XmlNode sibling = node.PreviousSibling;
            while (sibling != null)
            {
                if (sibling.Name == node.Name)
                {
                    number++;
                }
                sibling = sibling.PreviousSibling;
            }
            if (node.ParentNode != null && node.ParentNode.NodeType != XmlNodeType.Document)
            {
                GetXPathForNodeRecursive(node.ParentNode, xPathSB);
            }
            xPathSB.AppendFormat("/{0}[{1}]", node.LocalName, number);
        }

        private static bool IsSpecialAttribute(XmlAttribute attribute)
        {
            if (attribute.Name == XMLNS || attribute.Prefix == XMLNS)
            {
                return true;
            }
            return false;
        }
    }
}
