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
}
