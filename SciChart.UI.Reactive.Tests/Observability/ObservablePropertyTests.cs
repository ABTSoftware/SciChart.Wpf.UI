using System;
using NUnit.Framework;
using SciChart.UI.Reactive.Observability;

namespace SciChart.UI.Reactive.Tests.Observability
{
    [TestFixture]
    public class ObservablePropertyTests
    {
        public class SomeClass
        {
            public SomeClass()
            {
                SomeProp = new ObservableProperty<string>();
            }

            public ObservableProperty<string> SomeProp { get; }
        }

        [Test]
        public void ShouldGetSetProperty()
        {
            var test = new SomeClass();

            test.SomeProp.SetValue("Hello");

            string receivedValue = null;
            var token = test.SomeProp.Subscribe(t => receivedValue = t);
            Assert.That(receivedValue, Is.EqualTo("Hello"));

            test.SomeProp.SetValue("World");
            Assert.That(receivedValue, Is.EqualTo("World"));

            token.Dispose();
            test.SomeProp.SetValue("...");
            Assert.That(receivedValue, Is.EqualTo("World"));
        }
    }
}