using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SciChart.Wpf.UI.Controls.AttachedBehaviours
{
    /// <summary>
    /// Attached property to mimic radio grouping on toggle buttons
    /// Taken from here: http://www.bradcunningham.net/2009/09/grouping-and-checkboxes-in-wpf.html
    /// </summary>
    public class ToggleButtonExtensions
    {
        private static readonly Dictionary<String, List<ToggleButton>> _elementToGroupNames =
            new Dictionary<String, List<ToggleButton>>();

        /// <summary>
        /// Defines the GroupName DependenccyProperty
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.RegisterAttached("GroupName",
                typeof (String),
                typeof (ToggleButtonExtensions),
                new PropertyMetadata(String.Empty, OnGroupNameChanged));

        /// <summary>
        /// Sets the GroupName Attached Property
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetGroupName(ToggleButton element, String value)
        {
            element.SetValue(GroupNameProperty, value);
        }

        /// <summary>
        /// Gets the GroupName Attached Property
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static String GetGroupName(ToggleButton element)
        {
            return element.GetValue(GroupNameProperty).ToString();
        }

        private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Add an entry to the group name collection
            var toggleButton = d as ToggleButton;
            if (toggleButton == null) return;

            String newGroupName = e.NewValue.ToString();
            String oldGroupName = e.OldValue.ToString();

            if (String.IsNullOrEmpty(newGroupName))
            {
                //Removing the toggle button from grouping
                RemoveCheckboxFromGrouping(newGroupName, toggleButton);
            }
            else
            {
                //Switching to a new group
                if (newGroupName != oldGroupName)
                {
                    if (!String.IsNullOrEmpty(oldGroupName))
                    {
                        //Remove the old group mapping
                        RemoveCheckboxFromGrouping(oldGroupName, toggleButton);
                    }

                    AddCheckboxToGrouping(toggleButton, e.NewValue.ToString());
                }
            }
        }

        private static void RemoveCheckboxFromGrouping(string groupName, ToggleButton checkBox)
        {
            _elementToGroupNames[groupName].Remove(checkBox);
            if (_elementToGroupNames[groupName].Count == 0)
            {
                _elementToGroupNames.Remove(groupName);
            }

            checkBox.Click -= ToggleButtonChecked;
            checkBox.Checked -= ToggleButtonChecked;
            checkBox.Unloaded -= ToggleButtonUnloaded;
        }

        private static void AddCheckboxToGrouping(ToggleButton checkBox, string groupName)
        {
            List<ToggleButton> toggleButtons;
            if (!_elementToGroupNames.TryGetValue(groupName, out toggleButtons))
            {
                toggleButtons = new List<ToggleButton>();
                _elementToGroupNames.Add(groupName, toggleButtons);
            }

            _elementToGroupNames[groupName].Add(checkBox);

            checkBox.Click += ToggleButtonChecked;
            checkBox.Checked += ToggleButtonChecked;
            checkBox.Unloaded += ToggleButtonUnloaded;
        }

        private static void ToggleButtonUnloaded(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton) sender;
            RemoveCheckboxFromGrouping(GetGroupName(toggleButton), toggleButton);
        }

        private static void ToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            var toggleButton = e.OriginalSource as ToggleButton;

            var allToggleButtons = _elementToGroupNames[GetGroupName(toggleButton)];

            foreach (var item in allToggleButtons)
            {
                item.IsChecked = (item == toggleButton);
            }
        }
    }
}
