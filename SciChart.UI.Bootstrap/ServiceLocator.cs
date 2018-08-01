using Unity;

namespace SciChart.UI.Bootstrap
{
    public class ServiceLocator
    {
        private static readonly IUnityContainer _container = new UnityContainer();

        public static IUnityContainer Container { get { return _container; } }
    }
}