using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SciChart.UI.Reactive.Extensions;
using SciChart.UI.Reactive.Observability;

namespace SciChart.UI.Reactive.Tests.Extensions
{
    [TestFixture]
    public class ObservableExtensionsTests
    {
        [Test]
        public void WhenCollectionChanged_ShouldPublishObservable()
        {
            // Arrange
            ObservableCollection<string> collection = new ObservableCollection<string>();
            var obs = collection.WhenCollectionChanged();
            List<EventPattern<NotifyCollectionChangedEventArgs>> events =
                new List<EventPattern<NotifyCollectionChangedEventArgs>>();
            obs.Subscribe(e => events.Add(e));

            // Act/Assert
            collection.Add("Hello");
            Assert.That(events.Count, Is.EqualTo(1));
            Assert.That(events[0].Sender, Is.EqualTo(collection));
            Assert.That(events[0].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(events[0].EventArgs.NewItems.Contains("Hello"));

            collection.Add("World");
            Assert.That(events.Count, Is.EqualTo(2));
            Assert.That(events[1].Sender, Is.EqualTo(collection));
            Assert.That(events[1].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(events[1].EventArgs.NewItems.Contains("World"));

            collection.Remove("Hello");
            Assert.That(events.Count, Is.EqualTo(3));
            Assert.That(events[2].Sender, Is.EqualTo(collection));
            Assert.That(events[2].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(events[2].EventArgs.NewItems, Is.Null);
            Assert.That(events[2].EventArgs.OldItems.Contains("Hello"));

            collection[0] = "Goodbye!";
            Assert.That(events.Count, Is.EqualTo(4));
            Assert.That(events[3].Sender, Is.EqualTo(collection));
            Assert.That(events[3].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
            Assert.That(events[3].EventArgs.NewItems.Contains("Goodbye!"));
            Assert.That(events[3].EventArgs.OldItems.Contains("World"));

            collection.Clear();
            Assert.That(events.Count, Is.EqualTo(5));
            Assert.That(events[4].Sender, Is.EqualTo(collection));
            Assert.That(events[4].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));
        }

        public class TestClassWithObservableCollection : ObservableObjectBase
        {
            public ObservableCollection<string> ACollection
            {
                get => GetDynamicValue<ObservableCollection<string>>();
                set => SetDynamicValue(value);
            }
        }

        [Test]
        public void WhenCollectionOnClassChanged_ShouldPublishObservable()
        {
            // Arrange
            var testClass = new TestClassWithObservableCollection();
            var obs = testClass.WhenCollectionChanged(x => x.ACollection);
            List<EventPattern<NotifyCollectionChangedEventArgs>> events =
                new List<EventPattern<NotifyCollectionChangedEventArgs>>();
            obs.Subscribe(e => events.Add(e));

            Assert.That(events.Count, Is.EqualTo(1));
            Assert.That(events[0].Sender, Is.Null);
            Assert.That(events[0].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));
            events.Clear();

            // Act 1: First collection
            testClass.ACollection = new ObservableCollection<string>();
            Assert.That(events.Count, Is.EqualTo(1));
            Assert.That(events[0].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[0].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));

            // Act 2: Testing first collection
            events.Clear();

            testClass.ACollection.Add("Hello");
            Assert.That(events.Count, Is.EqualTo(1));
            Assert.That(events[0].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[0].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(events[0].EventArgs.NewItems.Contains("Hello"));

            testClass.ACollection.Add("World");
            Assert.That(events.Count, Is.EqualTo(2));
            Assert.That(events[1].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[1].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(events[1].EventArgs.NewItems.Contains("World"));

            testClass.ACollection.Remove("Hello");
            Assert.That(events.Count, Is.EqualTo(3));
            Assert.That(events[2].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[2].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(events[2].EventArgs.NewItems, Is.Null);
            Assert.That(events[2].EventArgs.OldItems.Contains("Hello"));

            testClass.ACollection[0] = "Goodbye!";
            Assert.That(events.Count, Is.EqualTo(4));
            Assert.That(events[3].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[3].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
            Assert.That(events[3].EventArgs.NewItems.Contains("Goodbye!"));
            Assert.That(events[3].EventArgs.OldItems.Contains("World"));

            testClass.ACollection.Clear();
            Assert.That(events.Count, Is.EqualTo(5));
            Assert.That(events[4].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[4].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));

            // Act 3: Second collection
            events.Clear();

            testClass.ACollection = new ObservableCollection<string>();
            Assert.That(events.Count, Is.EqualTo(1));
            Assert.That(events[0].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[0].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));

            // Act 4: Testing second collection
            events.Clear();

            testClass.ACollection.Add("Hello");
            Assert.That(events.Count, Is.EqualTo(1));
            Assert.That(events[0].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[0].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(events[0].EventArgs.NewItems.Contains("Hello"));

            testClass.ACollection.Add("World");
            Assert.That(events.Count, Is.EqualTo(2));
            Assert.That(events[1].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[1].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(events[1].EventArgs.NewItems.Contains("World"));

            testClass.ACollection.Remove("Hello");
            Assert.That(events.Count, Is.EqualTo(3));
            Assert.That(events[2].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[2].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(events[2].EventArgs.NewItems, Is.Null);
            Assert.That(events[2].EventArgs.OldItems.Contains("Hello"));

            testClass.ACollection[0] = "Goodbye!";
            Assert.That(events.Count, Is.EqualTo(4));
            Assert.That(events[3].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[3].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
            Assert.That(events[3].EventArgs.NewItems.Contains("Goodbye!"));
            Assert.That(events[3].EventArgs.OldItems.Contains("World"));

            testClass.ACollection.Clear();
            Assert.That(events.Count, Is.EqualTo(5));
            Assert.That(events[4].Sender, Is.EqualTo(testClass.ACollection));
            Assert.That(events[4].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));

            // Act 5: Nulling collection
            events.Clear();

            testClass.ACollection = null;

            Assert.That(events.Count, Is.EqualTo(1));
            Assert.That(events[0].Sender, Is.Null);
            Assert.That(events[0].EventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));
        }
    }
}
