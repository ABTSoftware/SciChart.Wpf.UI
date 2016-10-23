using System;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace SciChart.Wpf.UI.Reactive.Observability
{
    public static class ObservableObjectExtensions
    {
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