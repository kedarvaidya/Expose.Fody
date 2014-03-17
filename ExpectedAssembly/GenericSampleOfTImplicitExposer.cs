using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfTImplicitExposer<T> : IGenericSample<T>
	{
		private IGenericSample<T> innerSample;

		public GenericSampleOfTImplicitExposer(IGenericSample<T> sample)
		{
			this.innerSample = sample;
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

		public Tuple<T, U> Func<U>(T x, U y)
		{
			return innerSample.Func(x, y);
		}
		
		public event EventHandler<DummyEventArgs<T>> Event
		{
			add { innerSample.Event += value; }
			remove { innerSample.Event -= value; }
		}
	}
}
