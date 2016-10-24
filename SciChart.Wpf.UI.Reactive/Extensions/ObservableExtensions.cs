using System;
using System.Reactive.Linq;
using SciChart.Wpf.UI.Reactive.Async;

namespace SciChart.Wpf.UI.Reactive.Extensions
{
    public static class ObservableExtensions
    {
        public static IObservable<ExceptionOrResult<TResult>> SelectEx<T, TResult>(
            this IObservable<ExceptionOrResult<T>> source,
            Func<T, TResult> selector)
        {
            Validate.NotNull(source, "source");
            Validate.NotNull(selector, "selector");
            return source.Select(x =>
            {
                try
                {
                    if (x.IsFaulted)
                    {
                        return ExceptionOrResult<TResult>.Error(x.Exception);
                    }
                    return ExceptionOrResult<TResult>.Return(selector(x.Result));
                }
                catch (Exception ex)
                {
                    return ExceptionOrResult<TResult>.Error(ex);
                }
            });
        }

        public static IObservable<ExceptionOrResult<T>> WhereEx<T>(this IObservable<ExceptionOrResult<T>> source,
            Func<T, bool> predicate)
        {
            Validate.NotNull(source, "source");
            Validate.NotNull(predicate, "predicate");

            return source.Where(res =>
            {
                if (res.IsFaulted) return true;

                return predicate(res.Result);
            });
        }
    }
}
