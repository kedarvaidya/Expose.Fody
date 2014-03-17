using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfTImplicitExposer<T>
	{
		[ExposeInterfaceImplicitly]
		private IGenericSample<T> innerSample;

		public GenericSampleOfTImplicitExposer(IGenericSample<T> sample)
		{
			this.innerSample = sample;
		}
	}
}
