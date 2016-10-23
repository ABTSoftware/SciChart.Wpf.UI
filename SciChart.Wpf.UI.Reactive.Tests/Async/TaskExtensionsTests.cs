using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SciChart.Wpf.UI.Reactive.Async;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace SciChart.Wpf.UI.Reactive.Tests.Async
{
    [TestFixture]
    public class TaskExtensionsTests : ReactiveTest
    {
        [Test]
        public void WhenChainingTasksWithThenShouldChain()
        {
            // Arrange
            var stringBuilder = new StringBuilder();
            var s = new TaskImmediateScheduler();

            Action appendAction = () => stringBuilder.Append("Hello");

            // Act
            Task.Factory.StartNew(appendAction, s)
                .Then(() => stringBuilder.Append(" "), s)
                .Then(() => stringBuilder.Append("World"), s);

            // Assert
            Assert.That(stringBuilder.ToString(), Is.EqualTo("Hello World"));
        }

        [Test]
        public void WhenChainingTasksWithExceptionShouldCatch()
        {
            // Arrange
            var stringBuilder = new StringBuilder();
            var s = new TaskImmediateScheduler();

            Action appendAction = () => stringBuilder.Append("Hello");

            // Act
            Task.Factory.StartNew(appendAction, s)
                .Then(() =>
                {
                    throw new Exception(" Exception!");
                }, s)
                .Then(() => stringBuilder.Append("World"), s)
                .Catch(ex => stringBuilder.Append(ex.Flatten().InnerExceptions.First().Message), s);

            // Assert
            Assert.That(stringBuilder.ToString(), Is.EqualTo("Hello Exception!"));
        }

        [Test]
        public void WhenChainingTasksAsyncWithThenShouldChain()
        {
            // Arrange
            var stringBuilder = new StringBuilder();
            var waitHandle = new ManualResetEvent(false);

            Action appendAction = () => stringBuilder.Append("Hello");

            // Act
            Task.Factory.StartNew(appendAction)
                .Then(() => stringBuilder.Append(" "))
                .Then(() => stringBuilder.Append("World"))
                .Then(() => waitHandle.Set());

            waitHandle.WaitOne(1000);

            // Assert
            Assert.That(stringBuilder.ToString(), Is.EqualTo("Hello World"));
        }

        [Test]
        public void WhenChainingTasksAsyncWithExceptionShouldCatch()
        {
            // Arrange
            var stringBuilder = new StringBuilder();
            var waitHandle = new ManualResetEvent(false);

            Action appendAction = () => stringBuilder.Append("Hello");

            // Act
            Task.Factory.StartNew(appendAction)
                .Then(() =>
                {
                    throw new Exception(" Exception!");
                })
                .Then(() => stringBuilder.Append("World"))
                .Catch(ex =>
                {
                    stringBuilder.Append(ex.Flatten().InnerExceptions.First().Message);
                    waitHandle.Set();
                });

            waitHandle.WaitOne(5000);

            // Assert
            Assert.That(stringBuilder.ToString(), Is.EqualTo("Hello Exception!"));
        }

        [Test]
        public void WhenChainingTypedTasksWithThenShouldChain()
        {
            // Arrange
            var stringBuilder = new StringBuilder();
            var s = new TaskImmediateScheduler();

            // Act
            Task.Factory.StartNew(() => stringBuilder.Append("Hello"), s)
                .Then(sb => sb.Append(" "), s)
                .Then(sb => sb.Append("World"), s);

            // Assert
            Assert.That(stringBuilder.ToString(), Is.EqualTo("Hello World"));
        }

        [Test]
        public void WhenChainingTypedTasksWithExceptionShouldCatch()
        {
            // Arrange
            var stringBuilder = new StringBuilder();
            var s = new TaskImmediateScheduler();

            // Act
            Task.Factory.StartNew(() => stringBuilder.Append("Hello"), s)
                .Then(sb =>
                    {
                        throw new Exception(" Exception!");
                    }, s)
                .Then<StringBuilder>(sb => sb.Append("World"), s)
                .Catch(ex => stringBuilder.Append(ex.Flatten().InnerExceptions.First().Message), s);

            // Assert
            Assert.That(stringBuilder.ToString(), Is.EqualTo("Hello Exception!"));
        }

        [Test]
        public void WhenChainingTypedTasksWithReturnValueAndExceptionShouldCatch()
        {
            // Arrange
            var stringBuilder = new StringBuilder();
            var s = new TaskImmediateScheduler();

            // Act
            Task.Factory.StartNew(() => stringBuilder.Append("Hello"), s)
                .Then(sb =>
                {
                    throw new Exception(" Exception!");
                }, s)
                // ReSharper disable once RedundantTypeArgumentsOfMethod
                .Then<StringBuilder, StringBuilder>(sb =>
                {
                    // ReSharper disable once ConvertToLambdaExpression
                    return sb.Append("World");
                }, s)
                .Catch(ex => stringBuilder.Append(ex.Flatten().InnerExceptions.First().Message), s);

            // Assert
            Assert.That(stringBuilder.ToString(), Is.EqualTo("Hello Exception!"));
        }

        [Test]
        public void WhenChainingTypedTasksAsyncWithThenShouldChain()
        {
            // Arrange
            var stringBuilder = new StringBuilder();
            var waitHandle = new ManualResetEvent(false);

            // Act
            Task.Factory.StartNew(() => stringBuilder.Append("Hello"))
                .Then(sb => sb.Append(" "))
                .Then(sb => sb.Append("World"))
                .Then(_ => waitHandle.Set());

            waitHandle.WaitOne(1000);

            // Assert
            Assert.That(stringBuilder.ToString(), Is.EqualTo("Hello World"));
        }

        [Test]
        public void WhenChainingTypedTasksAsyncWithExceptionShouldCatch()
        {
            // Arrange
            var stringBuilder = new StringBuilder();
            var waitHandle = new ManualResetEvent(false);

            // Act
            Task.Factory.StartNew(() => stringBuilder.Append("Hello"))
                .Then(sb =>
                {
                    throw new Exception(" Exception!");
                })
                .Then<StringBuilder>(sb => sb.Append("World"))
                .Catch(ex =>
                    {
                        stringBuilder.Append(ex.Flatten().InnerExceptions.First().Message);
                        waitHandle.Set();
                    });

            waitHandle.WaitOne(5000);

            // Assert
            Assert.That(stringBuilder.ToString(), Is.EqualTo("Hello Exception!"));
        }

        [Test]
        public void WhenChainingTypedTasksAsyncWithReturnValueAndExceptionShouldCatch()
        {
            // Arrange
            var stringBuilder = new StringBuilder();
            var waitHandle = new ManualResetEvent(false);

            // Act
            Task.Factory.StartNew(() => stringBuilder.Append("Hello"))
                .Then(sb =>
                {
                    throw new Exception(" Exception!");
                })
                // ReSharper disable once RedundantTypeArgumentsOfMethod
                .Then<StringBuilder, StringBuilder>(sb =>
                {
                    // ReSharper disable once ConvertToLambdaExpression
                    return sb.Append("World");
                })
                .Catch(ex =>
                {
                    stringBuilder.Append(ex.Flatten().InnerExceptions.First().Message);
                    waitHandle.Set();
                });

            waitHandle.WaitOne(5000);

            // Assert
            Assert.That(stringBuilder.ToString(), Is.EqualTo("Hello Exception!"));
        }

        [Test]
        public void WhenTaskExFromResultShouldReturnResultImmediately()
        {
            // Arrange / Act
            var task = TaskEx.FromResult("Hello World");

            // Assert
            Assert.That(task.IsCompleted, Is.True);
            Assert.That(task.IsFaulted, Is.False);
            Assert.That(task.IsCanceled, Is.False);
            Assert.That(task.Result, Is.EqualTo("Hello World"));
        }

        [Test]
        public void WhenObservableToExceptionOrResultShouldReturnImmediately()
        {
            // Arrange
            var scheduler = new TestScheduler();
            var observable = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnNext(30, 20),
                OnCompleted<int>(40)
                );
            var observer = scheduler.CreateObserver<ExceptionOrResult<int>>();

            // Act
            observable.ToExceptionOrResult().Subscribe(observer);
            scheduler.Start();

            // Assert
            observer.Messages.AssertEqual(
                OnNext(10, new ExceptionOrResult<int>(10)),
                OnNext(30, new ExceptionOrResult<int>(20)),
                OnCompleted<ExceptionOrResult<int>>(40)
                );
        }

        [Test]
        public void WhenErroredObservableToExceptionOrResultThenReturnsExceptionOrResultError()
        {
            // Arrange
            var scheduler = new TestScheduler();
            var expectedException = new Exception();
            var observable = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnError<int>(30, expectedException)
                );
            var observer = scheduler.CreateObserver<ExceptionOrResult<int>>();

            // Act
            observable.ToExceptionOrResult().Subscribe(observer);
            scheduler.Start();

            // Assert
            observer.Messages.AssertEqual(
                OnNext(10, new ExceptionOrResult<int>(10)),
                OnNext(30, new ExceptionOrResult<int>(expectedException)),
                OnCompleted<ExceptionOrResult<int>>(30)
                );
        }
    }
}
