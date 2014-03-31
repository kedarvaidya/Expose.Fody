using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class HandCodedGenericSampleOfIntMembersExposer
	{
		private IGenericSample<int> innerSample;

		public HandCodedGenericSampleOfIntMembersExposer(IGenericSample<int> innerSample)
		{
			this.innerSample = innerSample;
		}

		public int ReadWriteValue
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

		public int Func(int x, int y)
		{
			return innerSample.Func(x, y);
		}

		public int this[int t]
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

		public event EventHandler<DummyEventArgs<int>> Event;

		public Tuple<int, U> Func<U>(int x, U y)
		{
			return innerSample.Func<U>(x, y);
		}

		public void RaiseEvent(int value)
		{
			if (Event != null)
			{
				Event(this, new DummyEventArgs<int>(value));
			}
		}
	}
}
