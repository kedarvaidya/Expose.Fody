using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class DummyEventArgs<T>: EventArgs
	{
		public T Value { get; private set; }

		public DummyEventArgs(T value)
		{
			Value = value;
		}
	}
}
