using SciChart.Wpf.UI.Reactive.Traits;

namespace SciChart.Wpf.UI.Reactive.Tests.Traits
{
    public class StubViewModelTrait : ViewModelTrait<StubObservableObject>
    {
        public StubViewModelTrait(StubObservableObject target) : base(target)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            IsDisposed = true;
        }

        public string Id { get; set; }        

        public bool IsDisposed { get; set; }
    }
}