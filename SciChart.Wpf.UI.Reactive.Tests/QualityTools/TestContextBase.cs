using System.ComponentModel;
using SciChart.Wpf.UI.Reactive.Async;
using SciChart.Wpf.UI.Reactive.Bootstrap;
using Microsoft.Practices.Unity;

namespace SciChart.Wpf.UI.Reactive.Tests.QualityTools
{
    public abstract class TestContextBase
    {
        protected TestContextBase()
        {
            Container = new UnityContainer();

            // Required for ViewModel BehaviourCollection
            ViewContext.Container = Container;

            SchedulerContext = new TestSchedulerContext();

            Container.RegisterInstance<ISchedulerContext>(SchedulerContext);
            Container.RegisterInstance<IUnityContainer>(Container);
        }

        public UnityContainer Container { get; protected set; }

        public TestSchedulerContext SchedulerContext { get; protected set; }
    }
}
