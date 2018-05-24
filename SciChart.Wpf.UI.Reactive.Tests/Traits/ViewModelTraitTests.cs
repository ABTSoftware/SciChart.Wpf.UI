using NUnit.Framework;
using Unity;

namespace SciChart.Wpf.UI.Reactive.Tests.Traits
{
    [TestFixture]
    public class ViewModelTraitTests
    {
        [Test]
        public void WhenDisposingTrait_ShouldDisposeChildren()
        {
            // Arrange
            var vm = new StubObservableObject();
            var trait = new StubViewModelTrait(vm);
            var aDisposableObject = new StubDisposable();

            aDisposableObject.DisposeWith(trait);

            trait.Dispose();

            Assert.That(trait.IsDisposed, Is.True, "Trait");
            Assert.That(aDisposableObject.IsDisposed, Is.True, "Child");
        }        
    }
}