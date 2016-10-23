using SciChart.Wpf.UI.Bootstrap;
using SciChart.Wpf.UI.Reactive.Behaviours;

namespace SciChart.Wpf.UI.Reactive.Observability
{
    public class ViewModelBase : ObservableObjectBase
    {
        public ViewModelBase()
        {
            Behaviours = new BehaviourCollection(this, ViewContext.Container);
        }

        public BehaviourCollection Behaviours { get; private set; }

        public T WithBehaviour<T>() where T : IBehaviour
        {
            return Behaviours.Add<T>();
        }

        public ExceptionViewModel Exception
        {
            get { return GetDynamicValue<ExceptionViewModel>("Exception"); }
            set { SetDynamicValue("Exception", value); }
        }
    }
}