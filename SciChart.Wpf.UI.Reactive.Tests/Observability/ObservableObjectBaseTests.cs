using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using SciChart.Wpf.UI.Reactive.Observability;
using NUnit.Framework;

namespace SciChart.Wpf.UI.Reactive.Tests.Observability
{
    [TestFixture]
    public class ObservableObjectBaseTests
    {
        private class MyObservableObject : ObservableObjectBase
        {
            private bool _isChecked;
            private string _something;

            public MyObservableObject(bool isChecked, string something)
            {
                _isChecked = isChecked;
                _something = something;
            }

            public bool IsChecked
            {
                get { return _isChecked; }
                set
                {
                    if (_isChecked == value) return;
                    _isChecked = value;
                    OnPropertyChanged("IsChecked", value);
                }
            }

            public string Something
            {
                get { return _something; }
                set
                {
                    if (_something == value) return;
                    _something = value;
                    OnPropertyChanged("Something", value);
                }
            }

            public object ADynamicValue
            {
                get { return GetDynamicValue<object>("ADynamicValue"); }
                set { SetDynamicValue("ADynamicValue", value);}
            }
        }

        [Test]
        public void ShouldSubscribeToPropertyChanges()
        {
            // Arrange
            bool boolReceived = false;
            string somethingReceived = null;
            var vm = new MyObservableObject(false, null);
            vm.WhenPropertyChanged(x => x.IsChecked).Subscribe(arg => boolReceived = arg);
            vm.WhenPropertyChanged(x => x.Something).Subscribe(arg => somethingReceived = arg);

            // Act
            vm.IsChecked = true;

            // Assert
            Assert.That(vm.IsChecked, Is.True);
            Assert.That(boolReceived, Is.True);
            Assert.That(somethingReceived, Is.Null);
        }

        [Test]
        public void ShouldSubscribeToPropertyChanges2()
        {
            // Arrange
            bool boolReceived = false;
            string somethingReceived = null;
            var vm = new MyObservableObject(false, null);
            vm.WhenPropertyChanged(x => x.IsChecked).Subscribe(arg => boolReceived = arg);
            vm.WhenPropertyChanged(x => x.Something).Subscribe(arg => somethingReceived = arg);

            // Act
            vm.Something = "Hello";
            vm.Something = "World";

            // Assert
            Assert.That(vm.IsChecked, Is.False);
            Assert.That(boolReceived, Is.False);
            Assert.That(somethingReceived, Is.EqualTo("World"));
        }

        [Test]
        public void ShouldDisposeWith()
        {
            // Arrange
            string somethingReceived = null;
            var vm = new MyObservableObject(false, null);
            vm.WhenPropertyChanged(x => x.Something).Subscribe(arg => somethingReceived = arg).DisposeWith(vm);

            // Act
            vm.Something = "Hello";
            vm.Dispose();
            vm.Something = "World";

            // Assert
            Assert.That(vm.IsChecked, Is.False);
            Assert.That(somethingReceived, Is.EqualTo("Hello"));
        }

        [Test]
        public void ShouldGetSetDynamicProperties()
        {
            // Arrange
            var obj = new MyObservableObject(false, null);

            // Act
            obj.SetDynamicValue<int>("SomeProp", 123);

            // Assert
            Assert.That(obj.GetDynamicValue<int>("SomeProp"), Is.EqualTo(123));
        }

        [Test]
        public void ShouldReturnDefaultOrNullForUninitializedDynamicProperties()
        {
            // Arrange
            var obj = new MyObservableObject(false, null);
            
            // Assert/Assert
            Assert.That(obj.GetDynamicValue<int>("SomeProp"), Is.EqualTo(default(int)));
            Assert.That(obj.GetDynamicValue<int?>("SomeOtherProp"), Is.Null);
            Assert.That(obj.GetDynamicValue<string>("SomethingElse"), Is.Null);
        }

        [Test]
        public void ShouldSetSetGetDynamicProperties()
        {
            // Arrange
            var obj = new MyObservableObject(false, null);

            // Act
            obj.SetDynamicValue<int>("SomeProp", 123);
            obj.SetDynamicValue<int>("SomeProp", 456);
            obj.SetDynamicValue<int>("SomeProp", 789);

            // Assert
            Assert.That(obj.GetDynamicValue<int>("SomeProp"), Is.EqualTo(789));
        }

        [Test]
        public void ShouldRaisePropertyChangedOnDynamicValueSet()
        {
            // Arrange
            var obj = new MyObservableObject(false, null);
            List<string> propertiesNotified = new List<string>();
            obj.PropertyChanged += (s, e) => propertiesNotified.Add(e.PropertyName);

            // Act
            obj.SetDynamicValue<int>("SomeProp", 123);
            obj.SetDynamicValue<int>("SomeOtherProp", 456);
            obj.SetDynamicValue<int>("SomeProp", 789);

            // Assert
            Assert.That(propertiesNotified.Count, Is.EqualTo(3));
            Assert.That(propertiesNotified, Is.EquivalentTo(new[] { "SomeProp", "SomeOtherProp", "SomeProp" }));
        }

        [Test]
        public void ShouldFireObservableEventsOnDynamicValueSet()
        {
            // Arrange
            var obj = new MyObservableObject(false, null);
            List<object> objectsReceived = new List<object>();
            obj.WhenPropertyChanged(x => x.ADynamicValue).Subscribe(v => objectsReceived.Add(v));
            objectsReceived.Clear();

            // Act
            obj.SetDynamicValue<object>("ADynamicValue", 123);
            obj.ADynamicValue = "Testing";
            obj.SetDynamicValue<object>("ADynamicValue", "Dynamic");

            // Assert
            Assert.That(objectsReceived.Count, Is.EqualTo(3));
            Assert.That(objectsReceived, Is.EquivalentTo(new object[] { 123, "Testing", "Dynamic" }));
        }

        [Test]
        public void ShouldNotRaisePropertyChangedOnDynamicValueSetToSameValue()
        {
            // Arrange
            var obj = new MyObservableObject(false, null);
            List<string> propertiesNotified = new List<string>();
            obj.PropertyChanged += (s, e) => propertiesNotified.Add(e.PropertyName);

            // Act
            obj.SetDynamicValue<string>("SomeProp", "Hi there!");
            obj.SetDynamicValue<string>("SomeProp", "Hi there!");
            obj.SetDynamicValue<int>("SomeOtherProp", 456);
            obj.SetDynamicValue<int>("SomeOtherProp", 456);
            obj.SetDynamicValue<bool?>("SomeNullableProp", true);
            obj.SetDynamicValue<bool?>("SomeNullableProp", true);

            // Assert
            Assert.That(propertiesNotified.Count, Is.EqualTo(3));
            Assert.That(propertiesNotified, Is.EquivalentTo(new[] { "SomeProp", "SomeOtherProp", "SomeNullableProp" }));
        }

        [Test]
        public void ShouldNotFireObservableEventsOnDynamicValueSetToSameValue()
        {
            // Arrange
            var obj = new MyObservableObject(false, null);
            List<object> objectsReceived = new List<object>();
            obj.WhenPropertyChanged(x => x.ADynamicValue).Subscribe(v => objectsReceived.Add(v));
            objectsReceived.Clear();

            // Act
            obj.SetDynamicValue<object>("ADynamicValue", 123);
            obj.ADynamicValue = "Testing";
            obj.SetDynamicValue<object>("ADynamicValue", "Dynamic");
            obj.SetDynamicValue<object>("ADynamicValue", "Dynamic");
            obj.ADynamicValue = "Dynamic";

            // Assert
            Assert.That(objectsReceived.Count, Is.EqualTo(3));
            Assert.That(objectsReceived, Is.EquivalentTo(new object[] { 123, "Testing", "Dynamic" }));
        }

        [Test]
        public void ShouldClearDynamicPropertiesOnDispose()
        {
            // Arrange
            var obj = new MyObservableObject(false, null);
            obj.SetDynamicValue<object>("ObjectProp", new ObservableObjectBase());
            Assert.That(obj.GetDynamicValue<object>("ObjectProp"), Is.Not.Null);

            // Act            
            obj.Dispose();

            // Assert
            Assert.That(obj.GetDynamicValue<object>("ObjectProp"), Is.Null);
        }
    }
}
