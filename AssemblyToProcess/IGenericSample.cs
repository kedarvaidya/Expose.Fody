using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public interface IGenericSample<T>
	{
		T ReadWriteValue { get; set; }

		int Func(T x, T y);

		T this[T t] { get; set; }

		event EventHandler<DummyEventArgs<T>> Event;

		Tuple<T, U> Func<U>(T x, U y);
	}
}
