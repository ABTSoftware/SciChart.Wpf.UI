using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using SciChart.UI.Reactive.Async;
using SciChart.UI.Reactive.Observability;

namespace SciChart.UI.Reactive.Extensions
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

        /// <summary>
        /// returns the IObservable&lt;EventPattern&lt;NotifyCollectionChangedEventArgs&gt;&gt; for collection Changed events on the ObservableCollection of type T
        /// </summary>        
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IObservable<EventPattern<NotifyCollectionChangedEventArgs>> WhenCollectionChanged(
            this INotifyCollectionChanged collection)
        {
            if (collection == null) return Observable.Empty<EventPattern<NotifyCollectionChangedEventArgs>>();

            return Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                h => collection.CollectionChanged += h,
                h => collection.CollectionChanged -= h);
        }

        /// <summary>
        /// Returns an IObservable&lt;EventPattern&lt;NotifyCollectionChangedEventArgs&gt;&gt; which also fires when the property instance is changed on a class.
        /// </summary>
        /// <example>
        /// <code>
        /// public class MyClass
        /// {
        ///    public ObservableColletion&lt;Object&gt; MyCollection { get;set; }
        /// }
        /// 
        /// Usage: MyClass.WhenCollectionChanged(x => x.MyCollection).Subscribe(..)
        /// </code>
        /// </example>
        /// <typeparam name="TContainer">The type of the containing object</typeparam>
        public static IObservable<EventPattern<NotifyCollectionChangedEventArgs>> WhenCollectionChanged<TContainer>(
            this TContainer parent, Expression<Func<TContainer, INotifyCollectionChanged>> selector)
            where TContainer : ObservableObjectBase
        {
            // When the parent TContainer property (ObservableCollection) instance changes
            var newCollectionObservable = parent.WhenPropertyChanged<TContainer, INotifyCollectionChanged>(selector);

            // ... get an IObservable<EventPattern> for the new collection
            var collectionChangedObservable = newCollectionObservable.Select(c => c.WhenCollectionChanged()).Switch();

            // But also publish an EventPattern<NotifyCollectionChangedEventArgs> with NotifyCollectionChangedAction.Reset because of the instance change 
            return newCollectionObservable
                .Select(x => new EventPattern<NotifyCollectionChangedEventArgs>(x, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)))
                .Merge(collectionChangedObservable);
        }
    }
}
