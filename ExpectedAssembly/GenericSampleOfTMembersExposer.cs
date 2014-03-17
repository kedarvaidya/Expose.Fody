using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfTMembersExposer<T>
	{
		private IGenericSample<T> innerSample;

		public GenericSampleOfTMembersExposer(IGenericSample<T> sample)
		{
			innerSample = sample;
		}

		public T ReadWriteValue { get { return innerSample.ReadWriteValue; } set { innerSample.ReadWriteValue = value; } }
		public T this[T t] { get { return innerSample[t]; } set { innerSample[t] = value; } }


		public int Func(T x, T y)
		{
			return innerSample.Func(x, y);
		}

		public Tuple<T, U> Func<U>(T x, U y)
		{
			return innerSample.Func<U>(x, y);
		}

		public event EventHandler<DummyEventArgs<T>> Event
		{
			add { innerSample.Event += value; }
			remove { innerSample.Event -= value; }
		}
	}
}
