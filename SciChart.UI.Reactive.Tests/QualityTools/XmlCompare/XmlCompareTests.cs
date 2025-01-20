using System.Collections.Generic;
using NUnit.Framework;

namespace SciChart.UI.Reactive.Tests.QualityTools.XmlCompare
{
    [TestFixture]
    [Category("DoNotRunOnBuildServer")] // fails on teamcity, but not locally 
    public class XmlCompareTests
    {
        private static readonly string expectedXml =
            @"<test attribute1='a'><element1>test text</element1><element1/><element2 otherAttribute='b'/></test>";

        private static readonly string oneElement1Missing =
            @"<test attribute1='a'><element1>test text</element1><element2 otherAttribute='b'/></test>";

        private static readonly string extraElement1 =
            @"<test attribute1='a'><element1>test text</element1><element1/><element1/><element2 otherAttribute='b'/></test>";

        private static readonly string extraTopLevelAttribute =
            @"<test attribute1='a' attribute2='c'><element1>test text</element1><element1/><element2 otherAttribute='b'/></test>";

        private static readonly string textMissing =
            @"<test attribute1='a'><element1></element1><element1/><element2 otherAttribute='b'/></test>";

        private static readonly string differentText =
            @"<test attribute1='a'><element1>other</element1><element1/><element2 otherAttribute='b'/></test>";

        private static readonly string attributeMissing =
            @"<test attribute1='a'><element1>test text</element1><element1/><element2/></test>";

        private class DifferenceHandler
        {
            private int nextCallback = 0;
            private List<string> expectedCallbacks = new List<string>();

            public void AddExpectedCallback(string callback)
            {
                expectedCallbacks.Add(callback);
            }

            public void Handle(object sender, XmlDifferentEventArgs args)
            {
                if (nextCallback >= expectedCallbacks.Count)
                {
                    Assert.Fail("Unexpected callback: {0}", args);
                }
                Assert.AreEqual(expectedCallbacks[nextCallback], args.ToString());
                nextCallback++;
                args.Handled = true;
            }

            public void VerifyAll()
            {
                Assert.AreEqual(expectedCallbacks.Count, nextCallback, "Expected {0} callbacks, but got only {1}.", expectedCallbacks.Count, nextCallback);
            }
        }

        private XmlComparer comparer;
        private DifferenceHandler handler;

        [SetUp]
        public void SetUp()
        {
            comparer = new XmlComparer(expectedXml);
            handler = new DifferenceHandler();
        }

        [Test]
        public void SameXmlDoesntThrow()
        {
            comparer.DifferenceFound += handler.Handle;
            comparer.Compare(expectedXml);
        }

        [Test]
        public void OneElement1MissingThrows()
        {
            Assert.Throws<XmlDifferentException>(() => comparer.Compare(oneElement1Missing), @"Xml documents were different: Difference type: DifferentNode, expected: element1, actual: element2
Position in expected document: /test[1]/element1[2]
Position in actual document: /test[1]/element2[1]");
        }

        [Test]
        public void OneElement1MissingCallback()
        {
            comparer.DifferenceFound += handler.Handle;
            handler.AddExpectedCallback(@"Difference type: DifferentNode, expected: element1, actual: element2
Position in expected document: /test[1]/element1[2]
Position in actual document: /test[1]/element2[1]");

            //These callbacks result from the fact that the code can't tell that this element was missing
            //it assumed there was a parallel shift
            handler.AddExpectedCallback(@"Difference type: ExtraAttribute, expected: , actual: 
Position in expected document: 
Position in actual document: /test[1]/element2[1]/@otherAttribute");
            handler.AddExpectedCallback(@"Difference type: MissingNode, expected: , actual: 
Position in expected document: /test[1]/element2[1]
Position in actual document: ");
            comparer.Compare(oneElement1Missing);
            handler.VerifyAll();
        }

        [Test]
        public void ExtraElement1Throws()
        {
            Assert.Throws<XmlDifferentException>(() => comparer.Compare(extraElement1), @"Xml documents were different: Difference type: DifferentNode, expected: element2, actual: element1
Position in expected document: /test[1]/element2[1]
Position in actual document: /test[1]/element1[3]");
        }

        [Test]
        public void ExtraElement1Callback()
        {
            comparer.DifferenceFound += handler.Handle;
            handler.AddExpectedCallback(@"Difference type: DifferentNode, expected: element2, actual: element1
Position in expected document: /test[1]/element2[1]
Position in actual document: /test[1]/element1[3]");

            //These callbacks result from the fact that the code can't tell that this element was missing
            //it assumed there was a parallel shift
            handler.AddExpectedCallback(@"Difference type: MissingAttribute, expected: , actual: 
Position in expected document: /test[1]/element2[1]/@otherAttribute
Position in actual document: ");
            handler.AddExpectedCallback(@"Difference type: ExtraNode, expected: , actual: 
Position in expected document: 
Position in actual document: /test[1]/element2[1]");
            comparer.Compare(extraElement1);
            handler.VerifyAll();
        }

        [Test]
        public void ExtraTopLevelAttributeThrows()
        {
            Assert.Throws<XmlDifferentException>(() => comparer.Compare(extraTopLevelAttribute), @"Xml documents were different: Difference type: ExtraAttribute, expected: , actual: 
Position in expected document: 
Position in actual document: /test[1]/@attribute2");
        }

        [Test]
        public void ExtraTopLevelAttributeCallback()
        {
            comparer.DifferenceFound += handler.Handle;
            handler.AddExpectedCallback(@"Difference type: ExtraAttribute, expected: , actual: 
Position in expected document: 
Position in actual document: /test[1]/@attribute2");
            comparer.Compare(extraTopLevelAttribute);
            handler.VerifyAll();
        }

        [Test]
        public void TextMissingThrows()
        {
            Assert.Throws<XmlDifferentException>(() => comparer.Compare(textMissing), @"Xml documents were different: Difference type: MissingNode, expected: , actual: 
Position in expected document: /test[1]/element1[1]/text()
Position in actual document: ");
        }

        [Test]
        public void TextMissingCallback()
        {
            comparer.DifferenceFound += handler.Handle;
            handler.AddExpectedCallback(@"Difference type: MissingNode, expected: , actual: 
Position in expected document: /test[1]/element1[1]/text()
Position in actual document: ");
            comparer.Compare(textMissing);
            handler.VerifyAll();
        }

        [Test]
        public void DifferentTextThrows()
        {
            Assert.Throws<XmlDifferentException>(() => comparer.Compare(differentText), @"Xml documents were different: Difference type: NodeValue, expected: test text, actual: other
Position in expected document: /test[1]/element1[1]/text()
Position in actual document: /test[1]/element1[1]/text()");
        }

        [Test]
        public void DifferentTextCallback()
        {
            comparer.DifferenceFound += handler.Handle;
            handler.AddExpectedCallback(@"Difference type: NodeValue, expected: test text, actual: other
Position in expected document: /test[1]/element1[1]/text()
Position in actual document: /test[1]/element1[1]/text()");
            comparer.Compare(differentText);
            handler.VerifyAll();
        }

        [Test]
        public void AttributeMissingThrows()
        {
            Assert.Throws<XmlDifferentException>(() => comparer.Compare(attributeMissing), @"Xml documents were different: Difference type: MissingAttribute, expected: , actual: 
Position in expected document: /test[1]/element2[1]/@otherAttribute
Position in actual document: ");
        }

        [Test]
        public void AttributeMissingCallback()
        {
            comparer.DifferenceFound += handler.Handle;
            handler.AddExpectedCallback(@"Difference type: MissingAttribute, expected: , actual: 
Position in expected document: /test[1]/element2[1]/@otherAttribute
Position in actual document: ");
            comparer.Compare(attributeMissing);
            handler.VerifyAll();
        }
    }
}