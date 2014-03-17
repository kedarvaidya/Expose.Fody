using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public interface ISample
	{
		double ReadWriteValue { get; set; }

		double this[double key] { get; set; }

		double Func(double x, double y);
	}
}
