using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SciChart.Wpf.UI.AttachedBehaviours
{
    /// <summary>
    /// DataGrid MultipleSelection behaviour from http://www.codeproject.com/Articles/412417/Managing-Multiple-selection-in-View-Model-NET-Metr
    /// </summary>
    public class DataGridMultiSelectBehaviour : Behavior<DataGrid>
    {
        private bool _selectionChangedInProgress;

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems",
                                    typeof(IList),
                                    typeof(DataGridMultiSelectBehaviour),
                                    new PropertyMetadata(default(IList), PropertyChangedCallback));

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItems == null) return;

            if (_selectionChangedInProgress) return;
            _selectionChangedInProgress = true;
            foreach (var item in e.RemovedItems)
            {
                if (SelectedItems.Contains(item))
                {
                    SelectedItems.Remove(item);
                }
            }

            foreach (var item in e.AddedItems)
            {
                if (!SelectedItems.Contains(item))
                {
                    SelectedItems.Add(item);
                }
            }
            _selectionChangedInProgress = false;
        }

        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            NotifyCollectionChangedEventHandler handler = (s, e) => SelectedItemsChanged(sender, e);
            if (args.OldValue is INotifyCollectionChanged)
            {
                (args.OldValue as INotifyCollectionChanged).CollectionChanged -= handler;
            }

            if (args.NewValue is INotifyCollectionChanged)
            {
                (args.NewValue as INotifyCollectionChanged).CollectionChanged += handler;
            }
        }

        private static void SelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is DataGridMultiSelectBehaviour)
            {
                var dataGrid = (sender as DataGridMultiSelectBehaviour).AssociatedObject;

                var listSelectedItems = dataGrid.SelectedItems;
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        if (listSelectedItems.Contains(item))
                        {
                            listSelectedItems.Remove(item);
                        }
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (!listSelectedItems.Contains(item))
                        {
                            listSelectedItems.Add(item);
                        }
                    }
                }
            }
        }
    }
}
