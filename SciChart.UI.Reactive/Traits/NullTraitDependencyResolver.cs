using SciChart.Wpf.UI.Reactive.Observability;

namespace SciChart.Wpf.UI.Reactive.Traits
{
    public class NullTraitDependencyResolver : ITraitDependencyResolver
    {
        public T ResolveWithParent<T>(ObservableObjectBase parent)
        {
            return default(T);
        }
    }
}