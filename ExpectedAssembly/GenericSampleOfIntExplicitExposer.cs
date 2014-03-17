using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfIntExplicitExposer : IGenericSample<int>
	{
		private IGenericSample<int> innerSample;

		public GenericSampleOfIntExplicitExposer(IGenericSample<int> sample)
		{
			this.innerSample = sample;
		}

		int IGenericSample<int>.ReadWriteValue
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

		int IGenericSample<int>.Func(int x, int y)
		{
			return innerSample.Func(x, y);
		}

		int IGenericSample<int>.this[int t]
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

		Tuple<int, U> IGenericSample<int>.Func<U>(int x, U y)
		{
			return innerSample.Func(x, y);
		}


		event EventHandler<DummyEventArgs<int>> IGenericSample<int>.Event
		{
			add { innerSample.Event += value; }
			remove { innerSample.Event -= value; }
		}
	}
}
