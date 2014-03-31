using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class HandCodedGenericSampleOfTMembersExposer<T>
	{
		private IGenericSample<T> innerSample;

		public HandCodedGenericSampleOfTMembersExposer(IGenericSample<T> innerSample)
		{
			this.innerSample = innerSample;
		}

		public T ReadWriteValue
		{
			get
			{
				return innerSample.ReadWriteValue;
			}
			set
			{
				innerSample.ReadWriteValue = value;
			}
		}

		public int Func(T x, T y)
		{
			return innerSample.Func(x, y);
		}

		public T this[T t]
		{
			get
			{
				return innerSample[t];
			}
			set
			{
				innerSample[t] = value;
			}
		}

		public event EventHandler<DummyEventArgs<T>> Event;

		public Tuple<T, U> Func<U>(T x, U y)
		{
			return innerSample.Func<U>(x, y);
		}

		public void RaiseEvent(T value)
		{
			if (Event != null)
			{
				Event(this, new DummyEventArgs<T>(value));
			}
		}
	}
}
