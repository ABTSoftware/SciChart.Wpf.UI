using System.ComponentModel;
using SciChart.Wpf.UI.Reactive.Async;
using Unity;
using SciChart.Wpf.UI.Bootstrap;
using SciChart.Wpf.UI.Reactive.Traits;

namespace SciChart.Wpf.UI.Reactive.Tests.QualityTools
{
    public abstract class TestContextBase
    {
        protected TestContextBase()
        {
            Container = new UnityContainer();

            // Required for ViewModel ViewModelTraitCollection
            ViewContext.Container = Container;

            SchedulerContext = new TestSchedulerContext();

            Container.RegisterInstance<ISchedulerContext>(SchedulerContext);
            Container.RegisterInstance<IUnityContainer>(Container);
            Container.RegisterInstance<ITraitDependencyResolver>(new TraitDependencyResolver(Container));
        }

        public UnityContainer Container { get; protected set; }

        public TestSchedulerContext SchedulerContext { get; protected set; }
    }
}
