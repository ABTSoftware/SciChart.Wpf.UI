using SciChart.Wpf.UI.Bootstrap;
using SciChart.Wpf.UI.Reactive.Traits;

namespace SciChart.Wpf.UI.Reactive.Observability
{
    /// <summary>
    /// Defines a ViewModel base class which extends <see cref="ObservableObjectBase"/> to allow Reactive (Rx) subscription to PropertyChanged
    /// events, as well as containing an inner <see cref="ViewModelTraitCollection"/> which allows for subsets of viewmodel behaviour to be neatly
    /// contained in classes which implement <see cref="IViewModelTrait"/>
    /// </summary>
    public class ViewModelWithTraitsBase : ObservableObjectBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelWithTraitsBase"/> class.
        /// </summary>
        public ViewModelWithTraitsBase()
        {
            ViewModelTraits = new ViewModelTraitCollection(this, ViewContext.Container);
        }

        /// <summary>
        /// Gets the <see cref="ViewModelTraitCollection"/> instance
        /// </summary>
        public ViewModelTraitCollection ViewModelTraits { get; private set; }

        /// <summary>
        /// Adds the type of Trait to the <see cref="ViewModelTraitCollection"/>. This will be instantiated with the current 
        /// dependency injection container and any dependencies of the Trait will be resolved and injected automatically. 
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IViewModelTrait"/> to add</typeparam>
        /// <returns>The <see cref="IViewModelTrait"/> instance</returns>
        public T WithTrait<T>() where T : IViewModelTrait
        {
            return ViewModelTraits.Add<T>();
        }

        public ExceptionViewModel Exception
        {
            get { return GetDynamicValue<ExceptionViewModel>(); }
            set { SetDynamicValue(value); }
        }
    }
}