using System.Reactive.Concurrency;

namespace SciChart.Wpf.UI.Reactive.Async
{
    public interface ISchedulerContext
    {
        SharedScheduler Default { get; }
        SharedScheduler Dispatcher { get; }
    }

    public class SchedulerContext : ISchedulerContext
    {
        private readonly SharedScheduler _default;
        private readonly SharedScheduler _dispatcher;

        public SchedulerContext(SharedScheduler dispatcher, SharedScheduler @default)
        {            
            _dispatcher = dispatcher;
            _default = @default;
        }

        public SharedScheduler Default { get { return _default; } }

        public SharedScheduler Dispatcher { get { return _dispatcher; } }
    }


//    public interface ISchedulerContext
//    {
//        ITaskScheduler Default { get; }
//        ITaskScheduler Dispatcher { get; }
//    }
//
//    public interface ITaskScheduler : IScheduler
//    {        
//    }
//
//    public abstract class SchedulerBase : TaskScheduler, ITaskScheduler
//    {
//        public abstract IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action);
//        public abstract IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action);
//        public abstract IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action);
//        public abstract DateTimeOffset Now { get; }
//    }
//
//    public class
//
//    public class SchedulerContext : ISchedulerContext
//    {
//        private readonly SchedulerBase _default;
//        private readonly SchedulerBase _dispatcher;
//
//        public SchedulerContext(SchedulerBase dispatcher, SchedulerBase @default)
//        {            
//            _dispatcher = dispatcher;
//            _default = @default;
//        }
//
//        public SchedulerBase Default { get { return _default; } }
//
//        public SchedulerBase Dispatcher { get { return _dispatcher; } }
//    }
}
