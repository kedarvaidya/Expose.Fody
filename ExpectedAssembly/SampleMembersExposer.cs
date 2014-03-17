using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class SampleMembersExposer
	{
		private ISample innerSample;

		public SampleMembersExposer(ISample sample)
		{
			this.innerSample = sample;
		}

		public double ReadWriteValue
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

		public double Func(double x, double y)
		{
			return innerSample.Func(x, y);
		}

		public double this[double key]
		{
			get
			{
				return innerSample[key];
			}
			set
			{
				innerSample[key] = value;
			}
		}
	}
}
