using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class SampleImpl: ISample
	{
		public double ReadWriteValue {get; set;}

		public double Func(double x, double y)
		{
			return x + y;
		}

		private Dictionary<double, double> _dict = new Dictionary<double, double>();
		public double this[double key]
		{
			get
			{
				return _dict[key];
			}
			set
			{
				_dict[key] = value;
			}
		}
	}
}
