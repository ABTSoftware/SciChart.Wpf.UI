using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using SciChart.UI.Reactive.Observability;

namespace SciChart.UI.Reactive.Async
{
    public static class ObservableEx
    {
        /// <summary>
        /// Invokes the specified function asynchronously on the specified scheduler, returning an <see cref="ExceptionOrResult{T}"/>
        /// through an observable sequence
        /// </summary>        
        public static IObservable<ExceptionOrResult<T>> Start<T>(Func<T> function, IScheduler scheduler)
        {
            Validate.NotNull(function, "function");
            Validate.NotNull(scheduler, "scheduler");

            return Observable.Start(() =>
                {
                    try
                    {
                        return new ExceptionOrResult<T>(function());
                    }
                    catch (Exception ex)
                    {
                        return new ExceptionOrResult<T>(ex);
                    }
                }, scheduler);
        }
    }
}
