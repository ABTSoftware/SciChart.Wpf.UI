using SciChart.UI.Reactive.Async;

namespace SciChart.UI.Reactive.Tests.QualityTools
{
    /// <summary>
    /// A <see cref="ISchedulerContext"/> implementation for unit tests. Both Background and Dispatcher Scheduler execute immediately
    /// </summary>
    public class TestSchedulerContext : ISchedulerContext
    {
        public TestSchedulerContext()
        {
            Default = new SharedScheduler(new TaskImmediateScheduler(), new RxImmediateScheduler());
            Dispatcher = new SharedScheduler(new TaskImmediateScheduler(), new RxImmediateScheduler());
        }

        public SharedScheduler Default { get; set; }
        public SharedScheduler Dispatcher { get; set; }
    }
}
