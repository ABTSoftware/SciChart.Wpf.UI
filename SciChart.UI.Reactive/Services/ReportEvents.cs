using System;
using System.Reactive.Subjects;
using SciChart.Wpf.UI.Bootstrap;

namespace SciChart.Wpf.UI.Reactive.Services
{
    public interface IReportEvents : IObservable<EventInfo>
    {        
        void Error(Exception ex);
        void Error(string message, Exception ex);
    }

    public class EventInfo
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }

    [ExportType(typeof(IReportEvents), CreateAs.Singleton)]
    public class ReportEvents : IReportEvents
    {
        private readonly Subject<EventInfo> _reportSubject = new Subject<EventInfo>();

        public void Error(Exception ex)
        {
            _reportSubject.OnNext(new EventInfo() { Message = null, Exception = ex });
        }

        public void Error(string message, Exception ex)
        {
            _reportSubject.OnNext(new EventInfo() { Message = message, Exception = ex });
        }

        public IDisposable Subscribe(IObserver<EventInfo> observer)
        {
            return _reportSubject.Subscribe(observer);
        }
    }
}
