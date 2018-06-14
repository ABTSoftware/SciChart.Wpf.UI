using System;
using System.Reactive.Concurrency;

namespace SciChart.Wpf.UI.Reactive.Tests.QualityTools
{
    /// <summary>
    /// A Reactive-Extensions Scheduler which processes everything immediately. Used for Unit Tests
    /// </summary>
    public class RxImmediateScheduler : IScheduler
    {
        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            return action(this, state);
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return action(this, state);
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return action(this, state);
        }

        public DateTimeOffset Now { get; private set; }
    }
}