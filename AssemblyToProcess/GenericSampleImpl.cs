using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleImpl<T> : IGenericSample<T>
	{
		public int Func(T x, T y)
		{
			return x.GetHashCode() + y.GetHashCode();
		}

		public T ReadWriteValue { get; set; }

		private Dictionary<T, T> _dict = new Dictionary<T, T>();
		public T this[T t]
		{
			get { return _dict[t]; }
			set { _dict[t] = value; }
		}

		public event EventHandler<DummyEventArgs<T>> Event;

		public void RaiseEvent(T value)
		{
			if (Event != null)
			{
				Event(this, new DummyEventArgs<T>(value));
			}
		}

		public Tuple<T, U> Func<U>(T x, U y)
		{
			return new Tuple<T, U>(x, y);
		}
	}
}
