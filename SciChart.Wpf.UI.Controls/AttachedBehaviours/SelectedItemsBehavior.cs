using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using SciChart.Wpf.UI.Reactive;

namespace SciChart.Wpf.UI.Controls.AttachedBehaviours
{
    /// <summary>
    /// A simple behaviour which injects DataGrid.SelectedItems into a ViewModel via an ActionCommand{T}
    /// </summary>
    public class SelectedItemsBehavior
    {
        public static readonly DependencyProperty SelectedItemsChangedHandlerProperty = DependencyProperty.RegisterAttached("SelectedItemsChangedHandler", typeof(ActionCommand<IEnumerable>), typeof(SelectedItemsBehavior), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemsChangedHandlerChanged)));

        public static ActionCommand<IEnumerable> GetSelectedItemsChangedHandler(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (ActionCommand<IEnumerable>)element.GetValue(SelectedItemsChangedHandlerProperty);
        }

        public static void SetSelectedItemsChangedHandler(DependencyObject element, ActionCommand<IEnumerable> value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(SelectedItemsChangedHandlerProperty, value);
        }


        private static void OnSelectedItemsChangedHandlerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;

            if (e.OldValue == null && e.NewValue != null)
            {
                dataGrid.SelectionChanged += ItemsControl_SelectionChanged;
            }

            if (e.OldValue != null && e.NewValue == null)
            {
                dataGrid.SelectionChanged -= ItemsControl_SelectionChanged;
            }

        }

        public static void ItemsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            ActionCommand<IEnumerable> itemsChangedHandler = GetSelectedItemsChangedHandler(dataGrid);

            itemsChangedHandler.Execute(dataGrid.SelectedItems);

        }

    }
}