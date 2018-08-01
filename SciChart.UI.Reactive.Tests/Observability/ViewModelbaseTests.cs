using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using NUnit.Framework;
using SciChart.UI.Reactive.Observability;
using SciChart.UI.Reactive.Tests.QualityTools;

namespace SciChart.UI.Reactive.Tests.Observability
{
    [TestFixture]
    public class ViewModelbaseTests
    {
        private TestSchedulerContext _ctx;

        private class MyViewModel : ViewModelBase
        {
            private bool _isChecked;
            private string _something;

            public MyViewModel(bool isChecked, string something)
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
                    OnPropertyChanged(value);
                }
            }

            public string Something
            {
                get { return _something; }
                set
                {
                    if (_something == value) return;
                    _something = value;
                    OnPropertyChanged(value);
                }
            }

            public object ADynamicValue
            {
                get { return GetDynamicValue<object>(); }
                set { SetDynamicValue(value);}
            }
        }

        [SetUp]
        public void Setup()
        {
            this._ctx = new TestSchedulerContext();
        }
      

        [Test]
        public void ShouldGetSetDynamicProperties()
        {
            // Arrange
            var obj = new MyViewModel(false, null);

            // Act
            obj.SetDynamicValue<int>(123, "SomeProp");

            // Assert
            Assert.That(obj.GetDynamicValue<int>("SomeProp"), Is.EqualTo(123));
        }

        [Test]
        public void ShouldReturnDefaultOrNullForUninitializedDynamicProperties()
        {
            // Arrange
            var obj = new MyViewModel(false, null);
            
            // Assert/Assert
            Assert.That(obj.GetDynamicValue<int>("SomeProp"), Is.EqualTo(default(int)));
            Assert.That(obj.GetDynamicValue<int?>("SomeOtherProp"), Is.Null);
            Assert.That(obj.GetDynamicValue<string>("SomethingElse"), Is.Null);
        }

        [Test]
        public void ShouldSetSetGetDynamicProperties()
        {
            // Arrange
            var obj = new MyViewModel(false, null);

            // Act
            obj.SetDynamicValue<int>(123, "SomeProp");
            obj.SetDynamicValue<int>(456, "SomeProp");
            obj.SetDynamicValue<int>(789, "SomeProp");

            // Assert
            Assert.That(obj.GetDynamicValue<int>("SomeProp"), Is.EqualTo(789));
        }

        [Test]
        public void ShouldRaisePropertyChangedOnDynamicValueSet()
        {
            // Arrange
            var obj = new MyViewModel(false, null);
            List<string> propertiesNotified = new List<string>();
            obj.PropertyChanged += (s, e) => propertiesNotified.Add(e.PropertyName);

            // Act
            obj.SetDynamicValue<int>(123, "SomeProp");
            obj.SetDynamicValue<int>(456, "SomeOtherProp");
            obj.SetDynamicValue<int>(789, "SomeProp");

            // Assert
            Assert.That(propertiesNotified.Count, Is.EqualTo(3));
            Assert.That(propertiesNotified, Is.EquivalentTo(new[] { "SomeProp", "SomeOtherProp", "SomeProp" }));
        }        

        [Test]
        public void ShouldNotRaisePropertyChangedOnDynamicValueSetToSameValue()
        {
            // Arrange
            var obj = new MyViewModel(false, null);
            List<string> propertiesNotified = new List<string>();
            obj.PropertyChanged += (s, e) => propertiesNotified.Add(e.PropertyName);

            // Act
            obj.SetDynamicValue<string>("Hi there!", "SomeProp");
            obj.SetDynamicValue<string>("Hi there!", "SomeProp");
            obj.SetDynamicValue<int>(456, "SomeOtherProp");
            obj.SetDynamicValue<int>(456, "SomeOtherProp");
            obj.SetDynamicValue<bool?>(true, "SomeNullableProp");
            obj.SetDynamicValue<bool?>(true, "SomeNullableProp");

            // Assert
            Assert.That(propertiesNotified.Count, Is.EqualTo(3));
            Assert.That(propertiesNotified, Is.EquivalentTo(new[] { "SomeProp", "SomeOtherProp", "SomeNullableProp" }));
        }                
    }
}