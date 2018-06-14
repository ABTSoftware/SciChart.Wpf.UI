using System;
using log4net;

namespace SciChart.Wpf.UI.Bootstrap
{
    /// <summary>
    /// A base class which implements IDisposable and implements the Finalizer pattern to ensure dispose is called by the Garbage Collector if not called by the user 
    /// </summary>
    public abstract class FinalizableObject : IDisposable
    {
        private static volatile bool _enableFinalizerLogging;

        private static readonly ILog Log = LogManager.GetLogger(typeof(FinalizableObject));

        /// <summary>
        /// Finalizes an instance of the <see cref="FinalizableObject"/> class.
        /// </summary>
        ~FinalizableObject()
        {
            if (EnableFinalizerLogging)
            {
                Log.InfoFormat("Type {0} was finalized. Consider using Dispose", GetType().Name);
            }

            Dispose(false);
        }

        /// <summary>
        /// When true, log Finalizer 
        /// </summary>
        public static bool EnableFinalizerLogging
        {
            get { return _enableFinalizerLogging; }
            set { _enableFinalizerLogging = value; }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}