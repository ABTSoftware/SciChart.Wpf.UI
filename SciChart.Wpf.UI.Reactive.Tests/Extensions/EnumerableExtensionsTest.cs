using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciChart.Wpf.UI.Reactive.Extensions;
using NUnit.Framework;

namespace SciChart.Wpf.UI.Reactive.Tests.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTest
    {
        private class AClass
        {
            public AClass(int id)
            {
                Id = id;
            }
            public int Id { get; set; }
        }
        [Test]
        public void ShouldReturnIndexOf()
        {
            // Arrange
            var input = new[] {new AClass(5), new AClass(2), new AClass(7), new AClass(11), new AClass(10)};

            // Act/Assert
            Assert.That(input.IndexOf(c => c.Id == 5), Is.EqualTo(0));
            Assert.That(input.IndexOf(c => c.Id == 2), Is.EqualTo(1));
            Assert.That(input.IndexOf(c => c.Id == 7), Is.EqualTo(2));
            Assert.That(input.IndexOf(c => c.Id == 11), Is.EqualTo(3));
            Assert.That(input.IndexOf(c => c.Id == 10), Is.EqualTo(4));
            Assert.That(input.IndexOf(c => c.Id == 12), Is.EqualTo(-1));
        }
    }

    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        [TestCase("abcdefg", "abcd_fg", true)]
        [TestCase("abcdefg", "ab%f%", true)]
        [TestCase("abcdefg", "ab*f*", true)]
        [TestCase("abcdefg", "*efg", true)]
        [TestCase("abcdefg", "efg", false)]
        [TestCase("abcdefg", "efgh", false)]
        [TestCase("abcdefg", "efg*h", false)]
        [TestCase("abcdefghi", "abcd_fg", false)]
        public void ShouldReturnStringLike(string input, string search, bool expectedResult)
        {
            Assert.That(input.Like(search), Is.EqualTo(expectedResult), "Input: {0}, Search: {1}, Expected, {2}", input, search, expectedResult);
        }
    }
}
