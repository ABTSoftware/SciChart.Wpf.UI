using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SciChart.Wpf.UI.Reactive;
using SciChart.Wpf.UI.Reactive.Observability;

namespace SciChart.Wpf.UI.Controls
{
    public class WarningsDialogViewModelDesignTime : WarningsDialogViewModel
    {
        public WarningsDialogViewModelDesignTime()
            : base(WarningDialogResult.Yes | WarningDialogResult.No | WarningDialogResult.Cancel)
        {
            Warnings.Add("This is a bad idea.");
            Warnings.Add("Something will go wrong and you will be really sorry!.");
        }
    }

    [Flags]
    public enum WarningDialogResult
    {
        Yes = 0x1,
        No = 0x2,
        Ok = 0x4,
        Cancel = 0x8
    }

    public class WarningsDialogViewModel : ViewModelBase
    {
        private readonly Subject<WarningDialogResult> _sub = new Subject<WarningDialogResult>();

        public WarningsDialogViewModel(WarningDialogResult buttons, params string[] warnings)
        {
            this.Buttons = buttons;
            this.YesCommand = new ActionCommand(() => _sub.OnNext(WarningDialogResult.Yes));
            this.NoCommand = new ActionCommand(() => _sub.OnNext(WarningDialogResult.No));
            this.OkCommand = new ActionCommand(() => _sub.OnNext(WarningDialogResult.Ok));
            this.CancelCommand = new ActionCommand(() => _sub.OnNext(WarningDialogResult.Cancel));
            this.Warnings = new ObservableCollection<string>(warnings);
            this.Title = "Do you want to proceed?";
        }

        public WarningsDialogViewModel(WarningDialogResult buttons, IEnumerable<string> warnings)
            : this(buttons, warnings != null ? warnings.ToArray() : new string[0])
        {
        }

        public string Title
        {
            get { return GetDynamicValue<string>("Title"); }
            set { SetDynamicValue<string>("Title", value); }
        }

        public WarningDialogResult Buttons { get; set; }
        public ActionCommand YesCommand { get; private set; }
        public ActionCommand NoCommand { get; private set; }
        public ActionCommand OkCommand { get; private set; }
        public ActionCommand CancelCommand { get; private set; }

        public ObservableCollection<string> Warnings { get; protected set; }

        public IObservable<WarningDialogResult> ResultObservable()
        {
            return _sub.AsObservable();
        }
    }
}
