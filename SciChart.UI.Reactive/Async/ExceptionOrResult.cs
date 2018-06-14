using System;
using System.Collections.Generic;

namespace SciChart.Wpf.UI.Reactive.Async
{    
    public class ExceptionOrResult<TResult>
    {
        private readonly TResult _result;
        private readonly Exception _exception;

        public ExceptionOrResult(TResult result)
        {
            _result = result;
        }

        public ExceptionOrResult(Exception ex)
        {
            _exception = ex;
        }

        public static ExceptionOrResult<TResult> Error(Exception ex)
        {
            return new ExceptionOrResult<TResult>(ex);
        }

        public static ExceptionOrResult<TResult> Return(TResult result = default(TResult))
        {
            return new ExceptionOrResult<TResult>(result);
        }

        public TResult Result
        {
            get
            {
                if (Exception != null)
                {
                    throw new InvalidOperationException("ExceptionOrResult has an error: ", Exception);
                }

                return _result;
            }
        }

        public bool IsFaulted { get { return _exception != null; } }

        public Exception Exception
        {
            get { return _exception; }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ExceptionOrResult<TResult>);
        }

        protected bool Equals(ExceptionOrResult<TResult> other)
        {
            if (other == null) return false;

            return EqualityComparer<TResult>.Default.Equals(_result, other._result) && 
                Equals(_exception, other._exception);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TResult>.Default.GetHashCode(_result) * 397) ^
                    (_exception != null ? _exception.GetHashCode() : 0);
            }
        }
    }
}
