using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciChart.UI.Reactive.Common;
using NUnit.Framework;

namespace SciChart.UI.Reactive.Tests.Common
{
    [TestFixture]
    public class StringlyTypedTests
    {
        public class IntegerType : StringlyTyped<int>
        {
            public IntegerType(int value) : base(value) { }            
        }

        public class StringType : StringlyTyped<string>
        {
            public StringType(string value) : base(value) { }
        }

        public class OtherStringType : StringlyTyped<string>
        {
            public OtherStringType(string value) : base(value) { }
        }

        [Test]
        public void ShouldCreateStringlyTyped()
        {
            var iType = new IntegerType(123);
            var sType = new StringType("Hello");
            var sType2 = new OtherStringType("World");

            Assert.That(iType.Value, Is.EqualTo(123));
            Assert.That(sType.Value, Is.EqualTo("Hello"));
            Assert.That(sType2.Value, Is.EqualTo("World"));

            Assert.That(iType, Is.Not.EqualTo(123));
            Assert.That(sType, Is.Not.EqualTo("Hello"));
            Assert.That(sType2, Is.Not.EqualTo("World"));
        }

        [Test]
        public void ShouldImplementEquality()
        {
            var hello = new StringType("Hello");
            var there = new StringType("There");
            var otherHello = new OtherStringType("Hello");
            var otherHello2 = new OtherStringType("Hello");

            Assert.That(hello.Equals(there), Is.False);
            Assert.That(hello.Equals(otherHello), Is.False);
            Assert.That(hello.Equals(otherHello2), Is.False);
            Assert.That(there.Equals(hello), Is.False);
            Assert.That(there.Equals(otherHello), Is.False);
            Assert.That(otherHello.Equals(otherHello2), Is.True);
        }

//        [Test]
//        public void ShouldImplementAssignment()
//        {
//            var iType = new IntegerType(123);
//            var sType = new StringType("Hello");
//            var sType2 = new OtherStringType("World");
//
//            int i = iType;
//            string s = sType;
//            string s2 = sType2;
//
//            Assert.That(i, Is.EqualTo(123));
//            Assert.That(s, Is.EqualTo("Hello"));
//            Assert.That(s2, Is.EqualTo("World"));
//        }
    }
}
