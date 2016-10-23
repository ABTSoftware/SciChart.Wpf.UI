using System;

namespace SciChart.Wpf.UI.Reactive
{
    public interface ICompositeDisposable : IDisposable
    {
        void AddDisposable(IDisposable disposable);
    }

    public static class DisposableExtensions
    {
        public static IDisposable DisposeWith(this IDisposable disposable, ICompositeDisposable compositeDisposable)
        {
            compositeDisposable.AddDisposable(disposable);
            return disposable;
        }
    }
}