using System;
using System.Collections.Generic;

namespace SciChart.Wpf.UI.Reactive.Common
{
    /// <summary>
    /// A base type for value types to avoid 'Stringly Typed' applications http://www.codinghorror.com/blog/2012/07/new-programming-jargon.html
    /// 
    /// Usage: Derive a class from StringlyTyped, e.g. <code>public class CustomerId : StringlyTyped<int></code>. Now you can use CustomerId 
    /// in methods that require an integer Id, and not end up with unmaintainable method calls like this:
    /// 
    /// <code>public void InsertCustomer(int id, int age, int orderId, int companyId)</code>
    /// 
    /// Instead it can be refactored like this:
    /// 
    /// <code>public void InsertCustomer(CustomerId id, Age age, OrderId orderId, CompanyId companyId)</code>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringlyTyped<T> where T:IComparable
    {
        public StringlyTyped(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        public override bool Equals(object obj)
        {            
            return this.Equals(obj as StringlyTyped<T>);
        }

        protected bool Equals(StringlyTyped<T> other)
        {
            if (other == null) return false;
            if (other.GetType() != this.GetType()) return false;

            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
