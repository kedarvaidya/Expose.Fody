using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfTExplicitExposer<T> : IGenericSample<T>
	{
		private IGenericSample<T> innerSample;

		public GenericSampleOfTExplicitExposer(IGenericSample<T> sample)
		{
			this.innerSample = sample;
		}

		T IGenericSample<T>.ReadWriteValue
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

		int IGenericSample<T>.Func(T x, T y)
		{
			return innerSample.Func(x, y);
		}

		T IGenericSample<T>.this[T t]
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

		Tuple<T, U> IGenericSample<T>.Func<U>(T x, U y)
		{
			return innerSample.Func(x, y);
		}

		event EventHandler<DummyEventArgs<T>> IGenericSample<T>.Event
		{
			add { innerSample.Event += value; }
			remove { innerSample.Event -= value; }
		}
	}
}
