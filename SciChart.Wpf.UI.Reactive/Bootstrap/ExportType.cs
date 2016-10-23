using System;

namespace SciChart.Wpf.UI.Reactive.Bootstrap
{
    public enum CreateAs
    {
        /// <summary>
        /// Creates a new instance each time this class is injected
        /// </summary>
        Default,

        /// <summary>
        /// Creates and stores the class as a singleton in the container
        /// </summary>
        Singleton
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]    
    public class ExportTypeAttribute : Attribute
    {
        private readonly Type _from;
        private CreateAs _createAs;

        public ExportTypeAttribute(Type @from, CreateAs createAs = CreateAs.Default)
        {
            _from = @from;
            _createAs = createAs;
        }

        public Type TFrom
        {
            get { return _from; }
        }

        public CreateAs CreateAs
        {
            get { return _createAs; }
        }
    }
}
