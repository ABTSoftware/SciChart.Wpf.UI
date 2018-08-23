using System.Threading;

namespace SciChart.UI.Reactive.Async
{
    /// <summary>
    /// Provides a <see cref="SynchronizationContext"/> implementation which executes all operations immediately (does not marshal to any specific thread)
    /// </summary>
    /// <seealso cref="System.Threading.SynchronizationContext" />
    public class ImmediateSynchronizationContext : SynchronizationContext
    {
        /// <summary>
        /// When overridden in a derived class, dispatches a synchronous message to a synchronization context.
        /// </summary>
        /// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback"></see> delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        public override void Send(SendOrPostCallback d, object state)
        {
            d.Invoke(state);
        }
    }
}