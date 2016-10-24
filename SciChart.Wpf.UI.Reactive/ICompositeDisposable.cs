using System;
using System.Reactive.Disposables;

namespace SciChart.Wpf.UI.Reactive
{
    /// <summary>
    /// The interface to a type which is <see cref="CompositeDisposable"/> - it composes a number of types which themselves are disposable. 
    /// 
    /// When this type is disposed, child types should also be disposed
    /// </summary>
    public interface ICompositeDisposable : IDisposable
    {
        /// <summary>
        /// Adds the disposable to the inner <see cref="CompositeDisposable"/>
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        void AddDisposable(IDisposable disposable);
    }

    public static class DisposableExtensions
    {
        /// <summary>
        /// Extension method to dispose the <see cref="IDisposable"/> disposable with the <see cref="ICompositeDisposable"/> instance
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        /// <param name="compositeDisposable">The composite disposable.</param>
        /// <returns></returns>
        public static IDisposable DisposeWith(this IDisposable disposable, ICompositeDisposable compositeDisposable)
        {
            compositeDisposable.AddDisposable(disposable);
            return disposable;
        }
    }
}