using Unity;
using Unity.Lifetime;

namespace SciChart.Wpf.UI.Bootstrap
{
    public static class UnityExtensions
    {
        public static void RegisterSingleton<TFrom, TTo>(this IUnityContainer container) where TTo:TFrom
        {
            container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
        }
    }
}