using System;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace SciChart.UI.Reactive.Observability
{
    public enum RxBindingMode
    {
        OneWay,
        OneWayToSource,        
        TwoWay,
    }
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
            where TViewModel : IObservableObject
        {
            var f = property.Body as MemberExpression;

            if (f == null)
                throw new NotSupportedException("Only use expressions that call a single property");

            var propertyName = f.Member.Name;
            var getValueFunc = property.Compile();

            return viewModel.PropertyChangedSubject.Where(x => x.Item1.Equals(propertyName)).Select(x => getValueFunc(viewModel)).StartWith(getValueFunc(viewModel));
        }

        public static void WhenPropertyChanged<TViewModel, TProp>(this TViewModel viewModel,
            Expression<Func<TViewModel, TProp>> whichProperty, Action<TProp> onPropertyChangedCallback)
            where TViewModel : IObservableObject
        {            
            viewModel.WhenPropertyChanged(whichProperty).Subscribe(onPropertyChangedCallback).DisposeWith(viewModel);
        }

//        public static IDisposable BindProperty<TViewModel1, TProp1, TViewModel2, TProp2>(this TViewModel1 source,
//            Expression<Func<TViewModel1, TProp1>> sourceProperty,
//            TViewModel2 dest, Expression<Func<TViewModel2, TProp2>> destProperty, RxBindingMode bindingMode = RxBindingMode.OneWay)
//            where TViewModel1 : ObservableObjectBase
//            where TViewModel2 : ObservableObjectBase
//        {
//            var sourceDispoisource.WhenPropertyChanged(sourceProperty).Subscribe();
//        }

        public static IObservable<Tuple<TProp1, TProp2>> WhenPropertiesChanged<TViewModel, TProp1, TProp2>(this TViewModel viewModel,
            Expression<Func<TViewModel, TProp1>> property1, 
            Expression<Func<TViewModel, TProp2>> property2)
            where TViewModel : IObservableObject
        {
            return Observable.CombineLatest(
                viewModel.WhenPropertyChanged(property1),
                viewModel.WhenPropertyChanged(property2),
                Tuple.Create);
        }

        public static IObservable<Tuple<TProp1, TProp2, TProp3>> WhenPropertiesChanged<TViewModel, TProp1, TProp2, TProp3>(this TViewModel viewModel,
            Expression<Func<TViewModel, TProp1>> property1,
            Expression<Func<TViewModel, TProp2>> property2,
            Expression<Func<TViewModel, TProp3>> property3)
            where TViewModel : IObservableObject
        {
            return Observable.CombineLatest(
                viewModel.WhenPropertyChanged(property1),
                viewModel.WhenPropertyChanged(property2),
                viewModel.WhenPropertyChanged(property3),
                Tuple.Create);
        }

        public static IObservable<Tuple<TProp1, TProp2, TProp3, TProp4>> WhenPropertiesChanged<TViewModel, TProp1, TProp2, TProp3, TProp4>(this TViewModel viewModel,
            Expression<Func<TViewModel, TProp1>> property1,
            Expression<Func<TViewModel, TProp2>> property2,
            Expression<Func<TViewModel, TProp3>> property3,
            Expression<Func<TViewModel, TProp4>> property4)
            where TViewModel : IObservableObject
        {
            return Observable.CombineLatest(
                viewModel.WhenPropertyChanged(property1),
                viewModel.WhenPropertyChanged(property2),
                viewModel.WhenPropertyChanged(property3),
                viewModel.WhenPropertyChanged(property4),
                Tuple.Create);
        }

        public static IObservable<Tuple<TProp1, TProp2, TProp3, TProp4, TProp5>> WhenPropertiesChanged<TViewModel, TProp1, TProp2, TProp3, TProp4, TProp5>(this TViewModel viewModel,
            Expression<Func<TViewModel, TProp1>> property1,
            Expression<Func<TViewModel, TProp2>> property2,
            Expression<Func<TViewModel, TProp3>> property3,
            Expression<Func<TViewModel, TProp4>> property4,
            Expression<Func<TViewModel, TProp5>> property5)
            where TViewModel : IObservableObject
        {
            return Observable.CombineLatest(
                viewModel.WhenPropertyChanged(property1),
                viewModel.WhenPropertyChanged(property2),
                viewModel.WhenPropertyChanged(property3),
                viewModel.WhenPropertyChanged(property4),
                viewModel.WhenPropertyChanged(property5),
                Tuple.Create);
        }
    }
}