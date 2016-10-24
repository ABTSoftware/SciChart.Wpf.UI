using SciChart.Wpf.UI.Reactive.Observability;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using SciChart.Wpf.UI.Reactive.Traits;

namespace SciChart.Wpf.UI.Reactive.Tests.Behaviours
{
    [TestFixture]
    public class BehaviourCollectionTests
    {
        public class MyObservableObject : ObservableObjectBase
        {            
        }

        public class MyViewModelTrait : ViewModelTrait<MyObservableObject>
        {
            public MyViewModelTrait(MyObservableObject target) : base(target)
            {
            }

            public string Id { get; set; }

            public override void Dispose()
            {
                base.Dispose();

                IsDisposed = true;
            }

            public bool IsDisposed { get; set; }
        }

        [Test]
        public void WhenAddBehaviourShouldAdd()
        {
            // Arrange
            var container = new UnityContainer();
            var parent = new MyObservableObject();
            var collection = new ViewModelTraitCollection(parent, container);

            // Act
            var b = collection.Add<MyViewModelTrait>();

            // Assert
            Assert.That(b, Is.Not.Null);
            Assert.That(collection.Contains<MyViewModelTrait>(), Is.True);
            Assert.That(b.Target, Is.EqualTo(parent));
        }

        [Test]
        public void WhenAddBehaviourTwiceShouldReplace()
        {
            // Arrange
            var container = new UnityContainer();
            var parent = new MyObservableObject();
            var collection = new ViewModelTraitCollection(parent, container);

            // Act
            var b0 = collection.Add<MyViewModelTrait>();
            b0.Id = "b0";
            var b1 = collection.Add<MyViewModelTrait>();
            b1.Id = "b1";

            // Assert
            Assert.That(b0, Is.Not.Null);
            Assert.That(b1, Is.Not.Null);
            Assert.That(collection.Contains<MyViewModelTrait>(), Is.True);
            Assert.That(b0.IsDisposed, Is.True);
            Assert.That(b1.IsDisposed, Is.False);
            Assert.That(b1.Target, Is.EqualTo(parent));
        }

        [Test]
        public void WhenDisposingParentShouldDispose()
        {
            // Arrange
            var container = new UnityContainer();
            var parent = new MyObservableObject();
            var collection = new ViewModelTraitCollection(parent, container);

            // Act
            var b0 = collection.Add<MyViewModelTrait>();
            parent.Dispose();

            // Assert            
            Assert.That(b0.IsDisposed, Is.True);
        }
    }
}
