using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class HandCodedSampleMembersExposer
	{
		[ExposeMembers]
		private ISample innerSample;

		public HandCodedSampleMembersExposer(ISample innerSample)
		{
			this.innerSample = innerSample;
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

		public double Func(double x, double y)
		{
			return innerSample.Func(x, y);
		}
	}
}
