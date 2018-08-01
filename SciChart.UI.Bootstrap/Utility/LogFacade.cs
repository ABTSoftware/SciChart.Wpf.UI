using System;

namespace SciChart.UI.Bootstrap.Utility
{
#if !SILVERLIGHT

    public class Log4NetFacade : ILogFacade
    {
        private readonly log4net.ILog _log4NetLog;

        static Log4NetFacade()
        {
#if !NETSTANDARD2_0
            log4net.Config.XmlConfigurator.Configure();
#endif           
        }

        public Log4NetFacade(log4net.ILog log4NetLog)
        {
            _log4NetLog = log4NetLog;
        }

        public void DebugFormat(string format, params object[] args)
        {
            try
            {
                _log4NetLog.DebugFormat(format, args);
            }
            catch
            {
            }
        }

        public void InfoFormat(string format, params object[] args)
        {
            try
            {
                _log4NetLog.InfoFormat(format, args);    
            }
            catch
            {
            }            
        }

        public void Error(Exception ex)
        {
            try
            {
                _log4NetLog.Error(ex);
            }
            catch
            {
            }
        }

        public void Debug(string str)
        {
            try
            {
                _log4NetLog.Debug(str);
            }
            catch (Exception)
            {                
            }
        }

        public void Error(string message, Exception ex)
        {
            try
            {
                _log4NetLog.Error(message, ex);
            }
            catch
            {                
            }
        }
    }
#endif

    public interface ILogFacade 
    {
        void DebugFormat(string format, params object[] args);
        void InfoFormat(string format, params object[] args);
        void Error(Exception ex);
        void Debug(string str);
        void Error(string message, Exception ex);
    }

    public class ConsoleLogFacade : ILogFacade
    {
        private readonly Type _type;

        public ConsoleLogFacade(Type type)
        {
            _type = type;
        }

        public void DebugFormat(string format, params object[] args)
        {
            String message = String.Format(format, args);
            Console.WriteLine("DEBUG [{0}, {1}]: {2}", _type.Name, DateTime.Now.ToString("hh:MM:ss"), message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            String message = String.Format(format, args);
            Console.WriteLine("INFO [{0}, {1}]: {2}", _type.Name, DateTime.Now.ToString("hh:MM:ss"), message);
        }

        public void Error(Exception ex)
        {
            String message = ex.Message;
            Console.WriteLine("ERROR [{0}, {1}]: {2}", _type.Name, DateTime.Now.ToString("hh:MM:ss"), message);
        }

        public void Debug(string str)
        {
            Console.WriteLine("DEBUG [{0}, {1}]: {2}", _type.Name, DateTime.Now.ToString("hh:MM:ss"), str);
        }

        public void Error(string message, Exception ex)
        {
            message = message + ".\r\n" + ex.Message;
            Console.WriteLine("ERROR [{0}, {1}]: {2}", _type.Name, DateTime.Now.ToString("hh:MM:ss"), message);
        }
    }

    public static class LogManagerFacade
    {
        public static ILogFacade GetLogger(Type type)
        {
#if !SILVERLIGHT
            return new Log4NetFacade(log4net.LogManager.GetLogger(type));
#else
            return new ConsoleLogFacade(type);
#endif
        }
    }    
}
