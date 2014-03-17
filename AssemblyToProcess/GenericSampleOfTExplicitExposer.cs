using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfTExplicitExposer<T>
	{
		[ExposeInterfaceExplicitly]
		private IGenericSample<T> innerSample;

		public GenericSampleOfTExplicitExposer(IGenericSample<T> sample)
		{
			this.innerSample = sample;
		}
	}
}
