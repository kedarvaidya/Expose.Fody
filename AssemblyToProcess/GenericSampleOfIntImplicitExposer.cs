using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfIntImplicitExposer
	{
		[ExposeInterfaceImplicitly]
		private IGenericSample<int> innerSample;

		public GenericSampleOfIntImplicitExposer(IGenericSample<int> sample)
		{
			this.innerSample = sample;
		}
	}
}
