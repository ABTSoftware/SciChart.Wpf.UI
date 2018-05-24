using SciChart.Wpf.UI.Reactive.Observability;

namespace SciChart.Wpf.UI.Reactive.Tests.Traits
{
    public class StubObservableObject : ObservableObjectBase
    {
        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }

        public bool IsDisposed { get; set; }
    }
}