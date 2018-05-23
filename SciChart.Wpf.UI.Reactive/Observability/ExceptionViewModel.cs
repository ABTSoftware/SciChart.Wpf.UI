using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace SciChart.Wpf.UI.Reactive.Observability
{
    public class ExceptionViewModel : ViewModelWithTraitsBase
    {
        public static string DefaultSupportEmailUri { get; set; }
        public static bool DefaultShowEmailSupport { get; set; }

        static ExceptionViewModel()
        {
            DefaultShowEmailSupport = true;
            DefaultSupportEmailUri = "mailto:support@scichart.com";
        }

        public ExceptionViewModel()
        {
            Observable.CombineLatest(
                    this.WhenPropertyChanged(x => x.Header),
                    this.WhenPropertyChanged(x => x.Exception),
                    Tuple.Create
                )
                .Subscribe(UpdateMessages)
                .DisposeWith(this);

            this.ShowEmailSupport = DefaultShowEmailSupport;
            this.SupportEmailUri = DefaultSupportEmailUri;
        }

        public ExceptionViewModel(string header, Exception exception)
            : this()
        {
            this.Header = header;
            this.Exception = exception;
        }

        public ExceptionViewModel(Exception exception)
            : this()
        {
            this.Exception = exception;
        }

        public string SupportEmailUri
        {
            get { return GetDynamicValue<string>(); }
            set { SetDynamicValue(value); }
        }

        public bool ShowEmailSupport
        {
            get { return GetDynamicValue<bool>(); }
            set { SetDynamicValue(value); }
        }

        public string Header
        {
            get { return GetDynamicValue<string>(); }
            set { SetDynamicValue(value); }
        }

        public Exception Exception
        {
            get { return GetDynamicValue<Exception>(); }
            set { SetDynamicValue(value); }
        }

        public IEnumerable<string> Messages
        {
            get { return GetDynamicValue<IEnumerable<string>>(); }
            set { SetDynamicValue(value); }
        }

        private void UpdateMessages(Tuple<string, Exception> t)
        {
            var messages = new List<string>();

            if (t.Item1 != null) messages.Add(t.Item1);
            if (t.Item2 != null)
            {
                Exception ex = t.Item2;
                while (ex != null)
                {
                    if (!string.IsNullOrEmpty(ex.Message) && !(ex is AggregateException))
                        messages.Add(string.Format("{0}: {1}", ex.GetType().Name, ex.Message));

                    ex = ex.InnerException;
                }
            }

            Messages = messages;
        }
    }
}