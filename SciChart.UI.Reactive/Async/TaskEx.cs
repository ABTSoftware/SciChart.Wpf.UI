using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace SciChart.UI.Reactive.Async
{
    public static class TaskEx
    {
        /// <summary>
        /// Returns a completed task with the result passed in
        /// </summary>
        public static Task<T> FromResult<T>(T result)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(result);
            return tcs.Task;
        }

        /// <summary>
        /// Starts a Task on the specified scheduler
        /// </summary>        
        public static Task<TResult> StartNew<TResult>(this TaskFactory factory, Func<TResult> operation,
                                             SharedScheduler scheduler = null)
        {
            var s = scheduler ?? TaskScheduler.Default;

            return new TaskFactory(s).StartNew(operation);
        }

        /// <summary>
        /// Starts a Task on the specified scheduler
        /// </summary>  
        public static Task<TResult> StartNew<TResult>(this TaskFactory factory, Func<TResult> operation,
                                             TaskScheduler scheduler = null)
        {
            var s = scheduler ?? TaskScheduler.Default;

            return new TaskFactory(s).StartNew(operation);
        }

        /// <summary>
        /// Continuation on a Task only if not faulted
        /// </summary>  
        public static Task<TResult> Then<TResult>(this Task<TResult> inputTask, Action<TResult> operation, TaskScheduler scheduler = null)
        {
            var s = scheduler ?? TaskScheduler.Default;
            
            var result = inputTask.ContinueWith(t =>
                {
                    // Progress continuation if not faulted
                    if (!inputTask.IsFaulted) operation(t.Result);
                    return t.Result;
                }, s);

            return result;
        }

        /// <summary>
        /// Continuation on a Task only if not faulted
        /// </summary>  
        public static Task<TResult2> Then<TResult, TResult2>(this Task<TResult> inputTask, Func<TResult, TResult2> operation, TaskScheduler scheduler = null)
        {
            var s = scheduler ?? TaskScheduler.Default;

            var result = inputTask.ContinueWith(t =>
            {
                // Progress continuation if not faulted
                if (!inputTask.IsFaulted)
                {
                    return operation(t.Result);
                }
                throw inputTask.Exception;
            }, s);

            return result;
        }

        /// <summary>
        /// Catches an exception that occurs anywhere in a continuation chain of Task.Then.Then.Catch
        /// </summary>
        public static void Catch<TResult>(this Task<TResult> inputTask, Action<AggregateException> operation, TaskScheduler scheduler = null)
        {
            var s = scheduler ?? TaskScheduler.Default;

            inputTask.ContinueWith(t =>
                {
                    if (t.Exception != null)
                    {
                        operation(t.Exception);
                    }
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, s);
        }

        /// <summary>
        /// Starts a Task on the specified scheduler
        /// </summary>     
        public static Task StartNew(this TaskFactory factory, Action operation,
                                             TaskScheduler scheduler = null)
        {
            var s = scheduler ?? TaskScheduler.Default;

            return new TaskFactory(s).StartNew(operation);
        }

        /// <summary>
        /// Continuation on a Task only if not faulted
        /// </summary>  
        public static Task Then(this Task inputTask, Action operation, TaskScheduler scheduler = null)
        {
            var s = scheduler ?? TaskScheduler.Default;

            var resultTask = inputTask.ContinueWith(t =>
            {                
                if (!inputTask.IsFaulted)
                {
                    // Progress continuation if not faulted
                    operation();
                }
                else
                {
                    // Progress Exception if faulted
                    throw inputTask.Exception;
                }
            }, s);

            return resultTask;
        }

        /// <summary>
        /// Catches an exception that occurs anywhere in a continuation chain of Task.Then.Then.Catch
        /// </summary>
        public static void Catch(this Task inputTask, Action<AggregateException> operation, TaskScheduler scheduler = null)
        {
            var s = scheduler ?? TaskScheduler.Default;

            inputTask.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    operation(t.Exception);
                }
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, s);
        }

        /// <summary>
        /// Converts an <see cref="IObservable{T}"/> to <see cref="IObservable{ExceptionOrResult{T}}"/> allowing error handling inside the stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IObservable<ExceptionOrResult<T>> ToExceptionOrResult<T>(this IObservable<T> source)
        {
            return Observable.Create<ExceptionOrResult<T>>(obs =>
                source.Subscribe(
                    x => obs.OnNext(new ExceptionOrResult<T>(x)),
                    ex =>
                    {
                        obs.OnNext(new ExceptionOrResult<T>(ex));
                        obs.OnCompleted();
                    },
                    obs.OnCompleted));

        }

        public static IObservable<ExceptionOrResult<T>> ToObservableEx<T>(this Task<T> task)
        {
            return task.ToObservable().ToExceptionOrResult();
        }

        public static IObservable<ExceptionOrResult<Unit>> ToObservableEx(this Task task)
        {
            return task.ToObservable().ToExceptionOrResult();
        }
    }
}
