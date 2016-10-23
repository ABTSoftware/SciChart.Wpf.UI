using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SciChart.Wpf.UI.Reactive.Bootstrap;
using SciChart.Wpf.UI.Reactive.Tests.Stubs;
using NUnit.Framework;

namespace SciChart.Wpf.UI.Reactive.Tests
{
    #region Support classes
    public class MyAttribute : Attribute { }

    [MyAttribute]
    public class DummyClass { }

    [MyAttribute]
    [OtherAttribute]
    public class AnotherDummyClass { }

    [OtherAttribute]
    public class YetAnotherDummyClass { }

    public class OtherAttribute : Attribute { }
    #endregion

    [TestFixture]
    public class AttributedTypeDiscoveryServiceTests
    {
        [Test]
        public void ShouldDiscoverAttributedClasses()
        {
            var currentAssembly = Assembly.GetAssembly(typeof(AttributedTypeDiscoveryServiceTests));
            var attributedTypeDiscovery = new AttributedTypeDiscoveryService(new StubAssemblyDiscovery(currentAssembly));

            var myAttributeTypes = attributedTypeDiscovery.DiscoverAttributedTypes<MyAttribute>();

            Assert.That(myAttributeTypes.Count(), Is.EqualTo(2));
            Assert.That(myAttributeTypes.First(), Is.EqualTo(typeof(DummyClass)));
            Assert.That(myAttributeTypes.Last(), Is.EqualTo(typeof(AnotherDummyClass)));

            var otherAttributeTypes = attributedTypeDiscovery.DiscoverAttributedTypes<OtherAttribute>();

            Assert.That(otherAttributeTypes.Count(), Is.EqualTo(2));
            Assert.That(otherAttributeTypes.First(), Is.EqualTo(typeof(AnotherDummyClass)));
            Assert.That(otherAttributeTypes.Last(), Is.EqualTo(typeof(YetAnotherDummyClass)));
        }
    }
}
