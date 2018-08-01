using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace SciChart.UI.Reactive.Async
{
    /// <summary>
    /// A shared Scheduler which can be passed to both RX methods and TPL methods 
    /// </summary>
    public class SharedScheduler : IScheduler
    {
        private readonly TaskScheduler _taskScheduler;
        private readonly IScheduler _rxScheduler;

        public SharedScheduler(TaskScheduler taskScheduler, IScheduler rxScheduler)
        {
            _taskScheduler = taskScheduler;
            _rxScheduler = rxScheduler;
        }

        public static implicit operator TaskScheduler(SharedScheduler s)
        {
            return s._taskScheduler;
        }

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            return _rxScheduler.Schedule(state, action);
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _rxScheduler.Schedule(state, dueTime, action);
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _rxScheduler.Schedule(state, dueTime, action);
        }

        public DateTimeOffset Now { get { return _rxScheduler.Now; } }
    }
}