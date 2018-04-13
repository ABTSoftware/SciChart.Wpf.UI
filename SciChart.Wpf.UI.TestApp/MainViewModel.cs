using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SciChart.Wpf.UI.Reactive;
using SciChart.Wpf.UI.Reactive.Observability;
using SciChart.Wpf.UI.Controls;

namespace SciChart.Wpf.UI.Controls.Preview
{
    public class MainViewModel : ViewModelWithTraitsBase
    {
        public MainViewModel()
        {            
            this.ThrowExceptionCommand = new ActionCommand(() => ThrowAnException());
            this.ShowAPopupCommand = new ActionCommand(() => IsPopupShown = true);
            this.HidePopupCommand = new ActionCommand(() => IsPopupShown = false);
            this.ShowWarningsCommand = new ActionCommand(() =>
                {
                    if (WarningsDialogViewModel != null) return;

                    WarningsDialogViewModel =
                        new WarningsDialogViewModel(WarningDialogResult.Ok | WarningDialogResult.Cancel,
                                                    "Are you really sure you want to do this?",
                                                    "If it breaks the build, then Andrew will cry.");
                    WarningsDialogViewModel.ResultObservable().Subscribe(_ =>
                        {
                            WarningsDialogViewModel = null;
                        }).DisposeWith(this);
                });

            ShowTransitionzContent = true;
        }

        private void ThrowAnException()
        {
            try
            {
                try
                {
                    throw new WebException("Unable to navigate to webpage, 'wonga.com'");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("An exception occurred all up in your app", ex);
                }
            }
            catch (Exception ex)
            {
                Exception = new ExceptionViewModel()
                {
                    Header = "An error has occurred",
                    Exception = ex,
                };
            }
        }

        public WarningsDialogViewModel WarningsDialogViewModel
        {
            get { return GetDynamicValue<WarningsDialogViewModel>("WarningsDialogViewModel"); }
            set { SetDynamicValue("WarningsDialogViewModel", value); }
        }

        public bool ShowTransitionzContent
        {
            get { return GetDynamicValue<bool>("ShowTransitionzContent"); }
            set { SetDynamicValue("ShowTransitionzContent", value); }
        }

        public bool IsPopupShown
        {
            get { return GetDynamicValue<bool>("IsPopupShown"); }
            set { SetDynamicValue("IsPopupShown", value); }
        }

        public ExceptionViewModel Exception
        {
            get { return GetDynamicValue<ExceptionViewModel>("Exception"); }
            set { SetDynamicValue("Exception", value); }
        }

        public ICommand HidePopupCommand
        {
            get { return GetDynamicValue<ICommand>("HidePopupCommand"); }
            set { SetDynamicValue("HidePopupCommand", value); }
        }

        public ICommand ThrowExceptionCommand
        {
            get { return GetDynamicValue<ICommand>("ThrowExceptionCommand"); }
            set { SetDynamicValue("ThrowExceptionCommand", value); }
        }

        public ICommand ShowAPopupCommand
        {
            get { return GetDynamicValue<ICommand>("ShowAPopupCommand"); }
            set { SetDynamicValue("ShowAPopupCommand", value); }
        }

        public ICommand ShowWarningsCommand
        {
            get { return GetDynamicValue<ICommand>("ShowWarningsCommand"); }
            set { SetDynamicValue("ShowWarningsCommand", value); }
        }
    }
}
