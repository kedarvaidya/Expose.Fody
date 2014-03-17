using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfIntMembersExposer
	{
		private IGenericSample<int> innerSample;

		public GenericSampleOfIntMembersExposer(IGenericSample<int> sample)
		{
			innerSample = sample;
		}

		public int ReadWriteValue { get { return innerSample.ReadWriteValue; } set { innerSample.ReadWriteValue = value; } }

		public int this[int t] { get { return innerSample[t]; } set { innerSample[t] = value; } }

		public int Func(int x, int y)
		{
			return innerSample.Func(x, y);
		}

		public Tuple<int, U> Func<U>(int x, U y)
		{
			return innerSample.Func(x, y);
		}

		public event EventHandler<DummyEventArgs<int>> Event
		{
			add { innerSample.Event += value; }
			remove { innerSample.Event -= value; }
		}
	}
}
