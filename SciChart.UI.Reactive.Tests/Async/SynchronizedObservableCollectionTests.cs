// ----------------------------------------------------------------------
// Derived from CCSWE.Core 
// https://github.com/CoryCharlton/CCSWE.Core/tree/master
// 
// Copyright Cory Charlton
// Apache License 2.0 https://github.com/CoryCharlton/CCSWE.Core/blob/master/LICENSE.md
// 
// Modifications by SciChart.Wpf.Ui team, copyright SCICHART Ltd (C) 2018
// ----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using SciChart.UI.Reactive.Async;

// ReSharper disable InconsistentNaming
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable UnusedVariable
namespace SciChart.UI.Reactive.Tests.Async
{
    [TestFixture]
    public class SynchronizedObservableCollectionTest
    {
        [TestFixture]
        public class When_Add_is_called
        {
            [Test]
            public void It_invokes_CollectionChanged()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                collection.Add("4");

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                Assert.That(collectionChangedEventArgs.NewItems[0], Is.EqualTo("4"));
                Assert.That(collectionChangedEventArgs.NewStartingIndex, Is.EqualTo(3));
            }

            [Test]
            public void It_invokes_PropertyChanged()
            {
                var collection = new SynchronizedObservableCollection<string>();
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                collection.Add("4");

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(2));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Count")), Is.True);
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }
        }

        [TestFixture]
        public class When_Clear_is_called
        {
            [Test]
            public void It_invokes_CollectionChanged()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                collection.Clear();

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));
            }

            [Test]
            public void It_invokes_PropertyChanged()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                collection.Clear();

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(2));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Count")), Is.True);
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }

            [Test]
            public void It_removes_all_items()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(collection.Count, Is.GreaterThan(0));
                collection.Clear();
                Assert.That(collection.Count, Is.EqualTo(0));
            }
        }

        [TestFixture]
        public class When_Constructor_is_called
        {
            [Test]
            public void It_does_not_throw_exception()
            {
                Assert.DoesNotThrow(() => new SynchronizedObservableCollection<string>());
            }

            [Test]
            public void It_does_not_throw_exception_when_collection_is_not_null()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(collection.Count, Is.EqualTo(3));
            }

            [Test]
            public void It_does_not_throw_exception_when_collection_and_context_are_not_null()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }, new SynchronizationContext());

                Assert.That(collection.Count, Is.EqualTo(3));
            }

            [Test]
            public void It_does_not_throw_exception_when_context_is_not_null()
            {
                Assert.DoesNotThrow(() => { new SynchronizedObservableCollection<string>(new SynchronizationContext()); });
            }

            [Test]
            public void It_throws_exception_when_collection_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new SynchronizedObservableCollection<string>((IEnumerable<string>)null));
                Assert.Throws<ArgumentNullException>(() => new SynchronizedObservableCollection<string>(null, new SynchronizationContext()));
            }

            [Test]
            public void It_throws_exception_when_context_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new SynchronizedObservableCollection<string>((SynchronizationContext)null));
                Assert.Throws<ArgumentNullException>(() => new SynchronizedObservableCollection<string>(new List<string>(), null));
            }
        }

        [TestFixture]
        public class When_Contains_is_called
        {
            [Test]
            public void It_returns_false_when_collection_does_not_contain_item()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(collection.Contains("4"), Is.False);
            }

            [Test]
            public void It_returns_true_when_collection_contains_item()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(collection.Contains("3"), Is.True);
            }
        }

        [TestFixture]
        public class When_CopyTo_is_called
        {
            [Test]
            public void It_copies_items_to_array()
            {
                var array = new string[3];
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.DoesNotThrow(() => collection.CopyTo(array, 0));
                Assert.That(array.Length, Is.EqualTo(3));
                Assert.That(array[0], Is.EqualTo("1"));
                Assert.That(array[1], Is.EqualTo("2"));
                Assert.That(array[2], Is.EqualTo("3"));
            }

            [Test]
            public void It_throws_exception_when_arrayIndex_is_less_than_zero()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentException>(() => collection.CopyTo(new string[1], -1));
            }

            [Test]
            public void It_throws_exception_when_arrayIndex_is_greater_than_or_equal_to_array_length()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentException>(() => collection.CopyTo(new string[1], 1));
            }

            [Test]
            public void It_throws_exception_when_array_is_null()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentNullException>(() => collection.CopyTo(null, 0));
            }

            [Test]
            public void It_throws_exception_when_offset_is_less_than_count()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentException>(() => collection.CopyTo(new string[3], 1));
            }
        }

        [TestFixture]
        public class When_Dispose_is_called
        {
            [Test]
            public void It_does_not_throw_exception()
            {
                Assert.DoesNotThrow(() => new SynchronizedObservableCollection<string>().Dispose());
            }

            [Test]
            public void It_does_not_throw_exception_if_already_disposed()
            {
                var collection = new SynchronizedObservableCollection<string>();

                Assert.DoesNotThrow(() => collection.Dispose());
                Assert.DoesNotThrow(() => collection.Dispose());
            }
        }

        [TestFixture]
        public class When_GetEnumerator_is_called
        {
            [Test]
            public void It_does_not_throw_exception_if_collection_is_modified()
            {
                var count = 0;
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                // ReSharper disable once UnusedVariable
                foreach (var item in collection)
                {
                    collection.Add($"{count + 4}");
                    count++;
                }

                Assert.That(count, Is.EqualTo(3));
                Assert.That(collection.Count, Is.EqualTo(6));
            }
        }

        [TestFixture]
        public class When_ICollection_CopyTo_is_called
        {
            [Test]
            public void It_copies_items_to_array()
            {
                var array = new string[3];
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.DoesNotThrow(() => ((ICollection)collection).CopyTo(array, 0));
                Assert.That(array.Length, Is.EqualTo(3));
                Assert.That(array[0], Is.EqualTo("1"));
                Assert.That(array[1], Is.EqualTo("2"));
                Assert.That(array[2], Is.EqualTo("3"));
            }

            [Test]
            public void It_throws_exception_when_arrayIndex_is_less_than_zero()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentException>(() => ((ICollection)collection).CopyTo(new string[1], -1));
            }

            [Test]
            public void It_throws_exception_when_arrayIndex_is_greater_than_or_equal_to_array_length()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentException>(() => ((ICollection)collection).CopyTo(new string[1], 1));
            }

            [Test]
            public void It_throws_exception_when_array_is_null()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.Throws<ArgumentNullException>(() => ((ICollection)collection).CopyTo(null, 0));
            }

            [Test]
            public void It_throws_exception_when_offset_is_less_than_count()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentException>(() => ((ICollection)collection).CopyTo(new string[3], 1));
            }
        }

        [TestFixture]
        public class When_ICollection_IsReadOnly_is_called
        {
            [Test]
            public void It_returns_false()
            {
                Assert.That(((ICollection<string>)new SynchronizedObservableCollection<string>()).IsReadOnly, Is.False);
            }
        }

        [TestFixture]
        public class When_ICollection_IsSynchronized_is_called
        {
            [Test]
            public void It_returns_true()
            {
                Assert.That(((ICollection)new SynchronizedObservableCollection<string>()).IsSynchronized, Is.True);
            }
        }

        [TestFixture]
        public class When_ICollection_SyncRoot_is_called
        {
            [Test]
            public void It_returns_non_null()
            {
                Assert.That(((ICollection)new SynchronizedObservableCollection<string>()).SyncRoot, Is.Not.Null);
            }
        }

        [TestFixture]
        public class When_IEnumerable_GetEnumerator_is_called
        {
            [Test]
            public void It_does_not_throw_exception_if_collection_is_modified()
            {
                var count = 0;
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                // ReSharper disable once UnusedVariable
                foreach (var item in (IEnumerable)collection)
                {
                    collection.Add($"{count + 4}");
                    count++;
                }

                Assert.That(count, Is.EqualTo(3));
                Assert.That(collection.Count, Is.EqualTo(6));
            }
        }

        [TestFixture]
        public class When_IList_Add_is_called
        {
            [Test]
            public void It_invokes_CollectionChanged()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                ((IList)collection).Add("4");

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                Assert.That(collectionChangedEventArgs.NewItems[0], Is.EqualTo("4"));
                Assert.That(collectionChangedEventArgs.NewStartingIndex, Is.EqualTo(3));
            }

            [Test]
            public void It_invokes_PropertyChanged()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                ((IList)collection).Add("4");

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(2));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Count")), Is.True);
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }

            [Test]
            public void It_throws_exception_when_value_is_not_compatible_object()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentException>(() => ((IList)collection).Add(1));
            }
        }

        [TestFixture]
        public class When_IList_Contains_is_called
        {
            [Test]
            public void It_returns_false_when_collection_does_not_contain_item()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(((IList)collection).Contains("4"), Is.False);
            }

            [Test]
            public void It_returns_false_when_value_is_not_compatible_object()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(((IList)collection).Contains(3), Is.False);
            }

            [Test]
            public void It_returns_true_when_collection_contains_item()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(((IList)collection).Contains("3"), Is.True);
            }
        }

        [TestFixture]
        public class When_IList_Indexer_Get_is_called
        {
            [Test]
            public void It_returns_item_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(((IList)collection)[2], Is.EqualTo("3"));
            }

            [Test]
            public void It_throws_exception_when_index_is_greater_than_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { var item = ((IList)new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }))[4]; });
            }

            [Test]
            public void It_throws_exception_when_index_is_equal_to_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { var item = ((IList)new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }))[3]; });
            }

            [Test]
            public void It_throws_exception_when_index_is_less_than_zero()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { var item = ((IList)new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }))[-1]; });
            }
        }

        [TestFixture]
        public class When_IList_Indexer_Set_is_called
        {
            [Test]
            public void It_invokes_CollectionChanged_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                ((IList)collection)[2] = "It_updates_item_when_index_is_in_range";

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
                Assert.That(collectionChangedEventArgs.NewItems[0], Is.EqualTo("It_updates_item_when_index_is_in_range"));
                Assert.That(collectionChangedEventArgs.NewStartingIndex, Is.EqualTo(2));
                Assert.That(collectionChangedEventArgs.OldItems[0], Is.EqualTo("3"));
                Assert.That(collectionChangedEventArgs.OldStartingIndex, Is.EqualTo(2));
            }

            [Test]
            public void It_invokes_PropertyChanged_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                ((IList)collection)[2] = "It_updates_item_when_index_is_in_range";

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(1));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }

            [Test]
            public void It_throws_exception_when_index_is_greater_than_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { ((IList)new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }))[4] = "When_Indexer_Set_is_called"; });
            }

            [Test]
            public void It_throws_exception_when_index_is_equal_to_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { ((IList)new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }))[3] = "When_Indexer_Set_is_called"; });
            }

            [Test]
            public void It_throws_exception_when_index_is_less_than_zero()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { ((IList)new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }))[-1] = "When_Indexer_Set_is_called"; });
            }

            [Test]
            public void It_updates_item_when_index_is_in_range()
            {
                // ReSharper disable once UseObjectOrCollectionInitializer
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                ((IList)collection)[2] = "It_updates_item_when_index_is_in_range";

                Assert.That(collection[2], Is.EqualTo("It_updates_item_when_index_is_in_range"));
            }
        }

        [TestFixture]
        public class When_IList_IndexOf_is_called
        {
            [Test]
            public void It_returns_index_of_item_when_item_is_found()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(((IList)collection).IndexOf("2"), Is.EqualTo(1));
            }

            [Test]
            public void It_returns_negative_one_when_item_is_not_compatible_object()
            {
                Assert.That(((IList)new SynchronizedObservableCollection<string>()).IndexOf(1), Is.EqualTo(-1));
            }

            [Test]
            public void It_returns_negative_one_when_item_is_not_found()
            {
                Assert.That(((IList)new SynchronizedObservableCollection<string>()).IndexOf("1"), Is.EqualTo(-1));
            }
        }

        [TestFixture]
        public class When_IList_Insert_is_called
        {
            [Test]
            public void It_inserts_the_item_when_index_is_equal_to_count()
            {
                var collection = new SynchronizedObservableCollection<string>();

                Assert.DoesNotThrow(() => { ((IList)collection).Insert(0, "0"); });
                Assert.That(collection.Count, Is.EqualTo(1));
            }

            [Test]
            public void It_inserts_the_item_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.DoesNotThrow(() => { ((IList)collection).Insert(1, "0"); });
                Assert.That(collection.Count, Is.EqualTo(4));
                Assert.That(collection[0], Is.EqualTo("1"));
                Assert.That(collection[1], Is.EqualTo("0"));
                Assert.That(collection[2], Is.EqualTo("2"));
                Assert.That(collection[3], Is.EqualTo("3"));
            }

            [Test]
            public void It_invokes_CollectionChanged_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                ((IList)collection).Insert(3, "4");

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                Assert.That(collectionChangedEventArgs.NewItems[0], Is.EqualTo("4"));
                Assert.That(collectionChangedEventArgs.NewStartingIndex, Is.EqualTo(3));
            }

            [Test]
            public void It_invokes_PropertyChanged_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                ((IList)collection).Insert(3, "4");

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(2));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Count")), Is.True);
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }

            [Test]
            public void It_throws_exception_when_index_is_greater_than_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { ((IList)new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" })).Insert(4, "4"); });
            }

            [Test]
            public void It_throws_exception_when_index_is_less_than_zero()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { ((IList)new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" })).Insert(-1, "4"); });
            }

            [Test]
            public void It_throws_exception_when_value_is_not_compatible_object()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentException>(() => ((IList)collection).Insert(1, 1));
            }
        }

        [TestFixture]
        public class When_IList_IsFixedSize_is_called
        {
            [Test]
            public void It_returns_false()
            {
                Assert.That(((IList)new SynchronizedObservableCollection<string>()).IsFixedSize, Is.False);
            }
        }

        [TestFixture]
        public class When_IList_IsReadOnly_is_called
        {
            [Test]
            public void It_returns_false()
            {
                Assert.That(((IList)new SynchronizedObservableCollection<string>()).IsReadOnly, Is.False);
            }
        }

        public class When_IList_Remove_is_called
        {
            [Test]
            public void It_invokes_CollectionChanged_when_collection_contains_item()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                ((IList)collection).Remove("3");

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                Assert.That(collectionChangedEventArgs.OldItems[0], Is.EqualTo("3"));
                Assert.That(collectionChangedEventArgs.OldStartingIndex, Is.EqualTo(2));
            }

            [Test]
            public void It_invokes_PropertyChanged_when_collection_contains_item()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3", "3" });
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                ((IList)collection).Remove("3");

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(2));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Count")), Is.True);
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }
        }

        [TestFixture]
        public class When_IndexOf_is_called
        {
            [Test]
            public void It_returns_index_of_item_when_item_is_found()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(collection.IndexOf("2"), Is.EqualTo(1));
            }

            [Test]
            public void It_returns_negative_one_when_item_is_not_found()
            {
                Assert.That(new SynchronizedObservableCollection<string>().IndexOf("1"), Is.EqualTo(-1));
            }
        }

        [TestFixture]
        public class When_Indexer_Get_is_called
        {
            [Test]
            public void It_returns_item_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(collection[2], Is.EqualTo("3"));
            }

            [Test]
            public void It_throws_exception_when_index_is_greater_than_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { var item = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" })[4]; });
            }

            [Test]
            public void It_throws_exception_when_index_is_equal_to_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { var item = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" })[3]; });
            }

            [Test]
            public void It_throws_exception_when_index_is_less_than_zero()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { var item = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" })[-1]; });
            }
        }

        [TestFixture]
        public class When_Indexer_Set_is_called
        {
            [Test]
            public void It_invokes_CollectionChanged_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                collection[2] = "It_updates_item_when_index_is_in_range";

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
                Assert.That(collectionChangedEventArgs.NewItems[0], Is.EqualTo("It_updates_item_when_index_is_in_range"));
                Assert.That(collectionChangedEventArgs.NewStartingIndex, Is.EqualTo(2));
                Assert.That(collectionChangedEventArgs.OldItems[0], Is.EqualTo("3"));
                Assert.That(collectionChangedEventArgs.OldStartingIndex, Is.EqualTo(2));
            }

            [Test]
            public void It_invokes_PropertyChanged_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                collection[2] = "It_updates_item_when_index_is_in_range";

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(1));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }

            [Test]
            public void It_throws_exception_when_index_is_greater_than_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" })[4] = "When_Indexer_Set_is_called"; });
            }

            [Test]
            public void It_throws_exception_when_index_is_equal_to_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" })[3] = "When_Indexer_Set_is_called"; });
            }

            [Test]
            public void It_throws_exception_when_index_is_less_than_zero()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" })[-1] = "When_Indexer_Set_is_called"; });
            }

            [Test]
            public void It_throws_exception_when_value_is_not_compatible_object()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentException>(() => ((IList)collection)[2] = 2);
            }

            [Test]
            public void It_updates_item_when_index_is_in_range()
            {
                // ReSharper disable once UseObjectOrCollectionInitializer
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                collection[2] = "It_updates_item_when_index_is_in_range";

                Assert.That(collection[2], Is.EqualTo("It_updates_item_when_index_is_in_range"));
            }
        }

        [TestFixture]
        public class When_Insert_is_called
        {
            [Test]
            public void It_inserts_the_item_when_index_is_equal_to_count()
            {
                var collection = new SynchronizedObservableCollection<string>();

                Assert.DoesNotThrow(() => { collection.Insert(0, "0"); });
                Assert.That(collection.Count, Is.EqualTo(1));
            }

            [Test]
            public void It_inserts_the_item_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.DoesNotThrow(() => { collection.Insert(1, "0"); });
                Assert.That(collection.Count, Is.EqualTo(4));
                Assert.That(collection[0], Is.EqualTo("1"));
                Assert.That(collection[1], Is.EqualTo("0"));
                Assert.That(collection[2], Is.EqualTo("2"));
                Assert.That(collection[3], Is.EqualTo("3"));
            }

            [Test]
            public void It_invokes_CollectionChanged_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                collection.Insert(3, "4");

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                Assert.That(collectionChangedEventArgs.NewItems[0], Is.EqualTo("4"));
                Assert.That(collectionChangedEventArgs.NewStartingIndex, Is.EqualTo(3));
            }

            [Test]
            public void It_invokes_PropertyChanged_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                collection.Insert(3, "4");

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(2));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Count")), Is.True);
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }

            [Test]
            public void It_throws_exception_when_index_is_greater_than_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }).Insert(4, "4"); });
            }

            [Test]
            public void It_throws_exception_when_index_is_less_than_zero()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }).Insert(-1, "4"); });
            }
        }

        [TestFixture]
        public class When_Move_is_called
        {
            [Test]
            public void It_invokes_CollectionChanged_when_new_index_and_old_index_are_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                collection.Move(0, 2);

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Move));
                Assert.That(collectionChangedEventArgs.OldItems[0], Is.EqualTo("1"));
                Assert.That(collectionChangedEventArgs.OldStartingIndex, Is.EqualTo(0));
                Assert.That(collectionChangedEventArgs.NewItems[0], Is.EqualTo("1"));
                Assert.That(collectionChangedEventArgs.NewStartingIndex, Is.EqualTo(2));
            }

            [Test]
            public void It_invokes_PropertyChanged_when_new_index_and_old_index_are_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                collection.Move(0, 2);

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(1));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }

            [Test]
            public void It_moves_the_item_when_new_index_and_old_index_are_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.DoesNotThrow(() => { collection.Move(0, 2); });
                Assert.That(collection.Count, Is.EqualTo(3));
                Assert.That(collection[0], Is.EqualTo("2"));
                Assert.That(collection[1], Is.EqualTo("3"));
                Assert.That(collection[2], Is.EqualTo("1"));
            }

            [Test]
            public void It_throws_exception_when_new_index_is_equal_to_count()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentOutOfRangeException>(() => { collection.Move(0, 3); });
            }

            [Test]
            public void It_throws_exception_when_new_index_is_greater_than_count()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentOutOfRangeException>(() => { collection.Move(0, 4); });
            }

            [Test]
            public void It_throws_exception_when_new_index_is_less_than_zero()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentOutOfRangeException>(() => { collection.Move(0, -1); });
            }

            [Test]
            public void It_throws_exception_when_old_index_is_equal_to_count()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentOutOfRangeException>(() => { collection.Move(3, 2); });
            }

            [Test]
            public void It_throws_exception_when_old_index_is_greater_than_count()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentOutOfRangeException>(() => { collection.Move(4, 2); });
            }

            [Test]
            public void It_throws_exception_when_old_index_is_less_than_zero()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentOutOfRangeException>(() => { collection.Move(-1, 2); });
            }
        }

        [TestFixture]
        public class When_Remove_is_called
        {
            [Test]
            public void It_invokes_CollectionChanged_when_collection_contains_item()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                collection.Remove("3");

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                Assert.That(collectionChangedEventArgs.OldItems[0], Is.EqualTo("3"));
                Assert.That(collectionChangedEventArgs.OldStartingIndex, Is.EqualTo(2));
            }

            [Test]
            public void It_invokes_PropertyChanged_when_collection_contains_item()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3", "3" });
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                collection.Remove("3");

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(2));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Count")), Is.True);
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }

            [Test]
            public void It_returns_false_when_collection_does_not_contain_item()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.That(collection.Remove("4"), Is.False);
                Assert.That(collection.Count, Is.EqualTo(3));
            }

            [Test]
            public void It_returns_true_when_collection_contains_item()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3", "3" });

                Assert.That(collection.Remove("3"), Is.True);
                Assert.That(collection.Count, Is.EqualTo(3));
                Assert.That(collection[0], Is.EqualTo("1"));
                Assert.That(collection[1], Is.EqualTo("2"));
                Assert.That(collection[2], Is.EqualTo("3"));
            }
        }

        [TestFixture]
        public class When_RemoveAt_is_called
        {
            [Test]
            public void It_invokes_CollectionChanges_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3", "3" });
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                collection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                collection.RemoveAt(2);

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                Assert.That(collectionChangedEventArgs.OldItems[0], Is.EqualTo("3"));
                Assert.That(collectionChangedEventArgs.OldStartingIndex, Is.EqualTo(2));
            }

            [Test]
            public void It_invokes_PropertyChanged_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3", "3" });
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                collection.RemoveAt(2);

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(2));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Count")), Is.True);
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }

            [Test]
            public void It_remove_the_item_when_index_is_in_range()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });

                Assert.DoesNotThrow(() => { collection.RemoveAt(1); });
                Assert.That(collection.Count, Is.EqualTo(2));
                Assert.That(collection[0], Is.EqualTo("1"));
                Assert.That(collection[1], Is.EqualTo("3"));
            }

            [Test]
            public void It_throws_exception_when_index_is_greater_than_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }).RemoveAt(4); });
            }

            [Test]
            public void It_throws_exception_when_index_is_equal_to_count()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }).RemoveAt(3); });
            }

            [Test]
            public void It_throws_exception_when_index_is_less_than_zero()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => { new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" }).RemoveAt(-1); });
            }
        }
    }
}
