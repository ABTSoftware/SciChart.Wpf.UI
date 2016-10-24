using System;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace SciChart.Wpf.UI.Reactive.Observability
{
    /// <summary>
    /// Extension methods for <see cref="ObservableObjectBase"/>
    /// </summary>
    public static class ObservableObjectExtensions
    {
        /// <summary>
        /// Transforms <see cref="ObservableObjectBase.PropertyChanged"/> into an <see cref="IObservable{TProperty}"/> for use in Reactive applications. 
        /// 
        /// Usage is: 
        /// <code>
        /// ObservableObjectBase o;
        /// o.WhenPropertyChanged(x => x.SomeProperty).Subscribe(...);
        /// </code>
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="viewModel">The view model instance.</param>
        /// <param name="property">The property to listen to (extracted via Expression / Func reflection).</param>
        /// <returns>An <see cref="IObservable{TProperty}"/> for this property</returns>
        /// <exception cref="System.NotSupportedException">Only use expressions that call a single property</exception>
        public static IObservable<TProp> WhenPropertyChanged<TViewModel, TProp>(this TViewModel viewModel, Expression<Func<TViewModel, TProp>> property)
            where TViewModel : ObservableObjectBase
        {
            var f = property.Body as MemberExpression;

            if (f == null)
                throw new NotSupportedException("Only use expressions that call a single property");

            var propertyName = f.Member.Name;
            var getValueFunc = property.Compile();

            return viewModel.PropertyChangedSubject.Where(x => x.Item1.Equals(propertyName)).Select(x => getValueFunc(viewModel)).StartWith(getValueFunc(viewModel));
        }
    }
}