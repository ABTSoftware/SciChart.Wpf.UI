using System.Text;
using System.Threading.Tasks;
using SciChart.UI.Reactive.Async;
using SciChart.UI.Reactive.Observability;
using SciChart.UI.Reactive.Tests.QualityTools;
using NUnit.Framework;

namespace SciChart.UI.Reactive.Tests.Async
{
    [TestFixture]
    public class SharedSchedulerTests
    {
        [Test]
        public void WhenUsingTplShouldBeAbleToUseSharedScheduler()
        {
            // Arrange
            var stringBuilder0 = new StringBuilder();
            var stringBuilder1 = new StringBuilder();
            var s = new SharedScheduler(new TaskImmediateScheduler(), new RxImmediateScheduler());

            // Act
            Task.Factory.StartNew(() => stringBuilder0.Append("Hello"), s)
                .Then(sb => sb.Append(" "), s)
                .Then(sb => sb.Append("World"), s);

            ObservableEx.Start(() => stringBuilder1.Append("Hi 2 yu!"), s);

            // Assert
            Assert.That(stringBuilder0.ToString(), Is.EqualTo("Hello World"));
            Assert.That(stringBuilder1.ToString(), Is.EqualTo("Hi 2 yu!"));
        }
    }
}