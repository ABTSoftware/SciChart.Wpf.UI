using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SciChart.UI.Reactive;
using SciChart.UI.Reactive.Observability;

namespace SciChart.Wpf.UI
{
    public class ExceptionViewer : Control
    {
        public static readonly DependencyProperty HasInnerExceptionsProperty =
            DependencyProperty.Register("HasInnerExceptions", typeof (bool), typeof (ExceptionViewer), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty ExceptionProperty =
            DependencyProperty.Register("Exception", typeof(ExceptionViewModel), typeof(ExceptionViewer), new FrameworkPropertyMetadata(default(ExceptionViewModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnExceptionViewModelChanged));

        public static readonly DependencyProperty ClearExceptionCommandProperty =
            DependencyProperty.Register("ClearExceptionCommand", typeof (ICommand), typeof (ExceptionViewer), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty ShowStackTraceProperty = DependencyProperty.Register("ShowStackTrace", typeof (bool), typeof (ExceptionViewer), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty SupportEmailUriProperty = DependencyProperty.Register(
            "SupportEmailUri", typeof(string), typeof(ExceptionViewer), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ShowEmailSupportProperty = DependencyProperty.Register(
            "ShowEmailSupport", typeof (bool), typeof (ExceptionViewer), new PropertyMetadata(default(bool)));

        public ExceptionViewer()
        {
            this.DefaultStyleKey = typeof (ExceptionViewer);            

            ClearExceptionCommand = new ActionCommand(() =>
            {
                SetCurrentValue(ExceptionProperty, null);
            });
        }

        public ICommand ClearExceptionCommand
        {
            get { return (ICommand)GetValue(ClearExceptionCommandProperty); }
            set { SetValue(ClearExceptionCommandProperty, value); }
        }   

        public ExceptionViewModel Exception
        {
            get { return (ExceptionViewModel)GetValue(ExceptionProperty); }
            set { SetValue(ExceptionProperty, value); }
        }

        public bool ShowStackTrace
        {
            get { return (bool)GetValue(ShowStackTraceProperty); }
            set { SetValue(ShowStackTraceProperty, value); }
        }

        public bool HasInnerExceptions
        {
            get { return (bool) GetValue(HasInnerExceptionsProperty); }
            set { SetValue(HasInnerExceptionsProperty, value); }
        }

        public string SupportEmailUri
        {
            get { return (string)GetValue(SupportEmailUriProperty); }
            set { SetValue(SupportEmailUriProperty, value); }
        }

        public bool ShowEmailSupport
        {
            get { return (bool)GetValue(ShowEmailSupportProperty); }
            set { SetValue(ShowEmailSupportProperty, value); }
        }

        private static void OnExceptionViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
