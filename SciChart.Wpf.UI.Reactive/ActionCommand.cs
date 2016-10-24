using System;
using System.Windows.Input;

namespace SciChart.Wpf.UI.Reactive
{
    /// <summary>
    /// Provides an ICommand derived class allowing delegates to be invokved directly on the view model 
    /// </summary>
    /// <typeparam name="T">The Type of the command parameter</typeparam>
    public class ActionCommand<T> : ICommand where T : class
    {
        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execute delegate.</param>
        /// <remarks></remarks>
        public ActionCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execute delegate.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        /// <remarks></remarks>
        public ActionCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute cannot be null");

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        /// <remarks></remarks>
        //[DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raises the CanExecuteChanged event
        /// </summary>
        /// <remarks></remarks>
        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <remarks></remarks>
        public void Execute(object parameter)
        {
            var param = parameter as T;

            if (parameter != null && param == null)
                throw new InvalidOperationException("Wrong type of parameter being passed in.  Expected [" + typeof(T) + "]but was [" + parameter.GetType() + "]");

            if (!CanExecute(param))
                throw new InvalidOperationException("Should not try to execute command that cannot be executed");

            _execute(param);
        }
    }

    /// <summary>
    /// Provides an ICommand derived class allowing delegates to be invokved directly on the view model 
    /// </summary>
    public class ActionCommand : ActionCommand<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute delegate.</param>
        /// <remarks></remarks>
        public ActionCommand(Action execute)
            : base(arg => execute())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute delegate.</param>
        /// <remarks></remarks>
        public ActionCommand(Action execute, Func<bool> canExecute)
            : base(arg => execute(), arg => canExecute())
        { }
    }
}
