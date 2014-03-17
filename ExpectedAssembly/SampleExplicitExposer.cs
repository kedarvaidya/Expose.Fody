using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class SampleExplicitExposer : ISample
	{
		private ISample innerSample;

		public SampleExplicitExposer(ISample sample)
		{
			this.innerSample = sample;
		}

		double ISample.ReadWriteValue
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

		double ISample.this[double key]
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

		double ISample.Func(double x, double y)
		{
			return innerSample.Func(x, y);
		}
	}
}
