using SciChart.UI.Reactive.Observability;

namespace SciChart.UI.Reactive.Traits
{
    public class NullTraitDependencyResolver : ITraitDependencyResolver
    {
        public T ResolveWithParent<T>(ObservableObjectBase parent)
        {
            return default(T);
        }
    }
}