using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SciChart.Wpf.UI.Reactive.Extensions;
using NUnit.Framework;

namespace SciChart.Wpf.UI.Reactive.Tests.QualityTools
{
    public static class ObjectExtensions
    {
        public static void AssertAllPropertiesInitialize(this object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ForEachDo(p =>
                {
                    var value = p.GetValue(obj, null);
                    object defaultValue = null;
                    if (p.PropertyType.IsValueType)
                    {
                        defaultValue = Activator.CreateInstance(p.PropertyType);
                    }

                    if (value == null || value.Equals(defaultValue))
                    {
                        Assert.Fail("The property {0} is not initialized", p.Name);
                    }
                });
        }
    }

    [TestFixture]
    public class ObjectExtensionsTests
    {
        public class SomeClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public IDisposable ReferenceType { get; set; } 
        }

        [Test]
        public void ShouldAssertAllPropertiesInitialized()
        {
            // Arrange
            var initializedClass = new SomeClass() {Id = 1, Name = "dave", ReferenceType = new MemoryStream()};
            var unnitializedClass0 = new SomeClass() { Id = 1, Name = "dave", ReferenceType = null };
            var unnitializedClass1 = new SomeClass() { Name = "dave", ReferenceType = new MemoryStream() };
            var unnitializedClass2 = new SomeClass() { Id = 1, ReferenceType = new MemoryStream() };

            // Act/Assert
            Assert.DoesNotThrow(initializedClass.AssertAllPropertiesInitialize);
            Assert.Throws<AssertionException>(() => unnitializedClass0.AssertAllPropertiesInitialize());
            Assert.Throws<AssertionException>(() => unnitializedClass1.AssertAllPropertiesInitialize());
            Assert.Throws<AssertionException>(() => unnitializedClass2.AssertAllPropertiesInitialize());
        }
    }
}
