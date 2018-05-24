using System;

namespace SciChart.Wpf.UI.Reactive.Tests.Traits
{
    public class StubDisposable : IDisposable
    {
        public void Dispose()
        {
            IsDisposed = true;
        }

        public bool IsDisposed { get; set; }
    }
}