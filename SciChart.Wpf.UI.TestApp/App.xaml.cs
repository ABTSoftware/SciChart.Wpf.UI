using System.Windows;
using SciChart.UI.Bootstrap;
using Unity;

namespace SciChart.Wpf.UI.TestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AbtBootstrapper _bootstrapper;

        public App()
        {
            _bootstrapper = new AbtBootstrapper(new UnityContainer(), new AttributedTypeDiscoveryService(new AutoAssemblyDiscovery()));
            _bootstrapper.Initialize();
        }
    }
}
