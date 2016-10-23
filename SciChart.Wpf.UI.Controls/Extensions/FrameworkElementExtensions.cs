using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace System.Windows
{
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Gets a value indicating whether this element has been loaded for presentation 
        /// Silverlight & WPF Compatible Implementation
        /// </summary>
        public static bool IsLoaded(this FrameworkElement fe)
        {
#if SILVERLIGHT
            var parent = System.Windows.Media.VisualTreeHelper.GetParent(fe);
            return parent != null;
#else
            return fe.IsLoaded;
#endif
        }

        internal static void BeginInvoke(this FrameworkElement fe, Action action, DispatchPriority dispatcherPriority)
        {
            if (fe == null) throw new ArgumentNullException("fe");

            var dispatcherInstance = fe.Dispatcher;

            if (dispatcherInstance == null)
            {
                action();
                return;
            }

#if !SILVERLIGHT
            dispatcherInstance.BeginInvoke((DispatcherPriority)dispatcherPriority, action);
#else
            dispatcherInstance.BeginInvoke(action);
#endif
        }
    }

    // Summary:
    //     Describes the priorities at which operations can be invoked by way of the
    //     System.Windows.Threading.Dispatcher.
    internal enum DispatchPriority
    {
        // Summary:
        //     The enumeration value is -1. This is an invalid priority.
        Invalid = -1,
        //
        // Summary:
        //     The enumeration value is 0. Operations are not processed.
        Inactive = 0,
        //
        // Summary:
        //     The enumeration value is 1. Operations are processed when the system is idle.
        SystemIdle = 1,
        //
        // Summary:
        //     The enumeration value is 2. Operations are processed when the application
        //     is idle.
        ApplicationIdle = 2,
        //
        // Summary:
        //     The enumeration value is 3. Operations are processed after background operations
        //     have completed.
        ContextIdle = 3,
        //
        // Summary:
        //     The enumeration value is 4. Operations are processed after all other non-idle
        //     operations are completed.
        Background = 4,
        //
        // Summary:
        //     The enumeration value is 5. Operations are processed at the same priority
        //     as input.
        Input = 5,
        //
        // Summary:
        //     The enumeration value is 6. Operations are processed when layout and render
        //     has finished but just before items at input priority are serviced. Specifically
        //     this is used when raising the Loaded event.
        Loaded = 6,
        //
        // Summary:
        //     The enumeration value is 7. Operations processed at the same priority as
        //     rendering.
        Render = 7,
        //
        // Summary:
        //     The enumeration value is 8. Operations are processed at the same priority
        //     as data binding.
        DataBind = 8,
        //
        // Summary:
        //     The enumeration value is 9. Operations are processed at normal priority.
        //     This is the typical application priority.
        Normal = 9,
        //
        // Summary:
        //     The enumeration value is 10. Operations are processed before other asynchronous
        //     operations. This is the highest priority.
        Send = 10,
    }
}
