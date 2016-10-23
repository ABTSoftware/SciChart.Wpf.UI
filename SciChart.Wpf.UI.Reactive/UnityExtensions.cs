using Microsoft.Practices.Unity;

namespace SciChart.Wpf.UI.Reactive
{
    public static class UnityExtensions
    {
        public static void RegisterSingleton<TFrom, TTo>(this IUnityContainer container) where TTo:TFrom
        {
            container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
        }
    }
}