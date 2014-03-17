using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfIntExplicitExposer
	{
		[ExposeInterfaceExplicitly]
		private IGenericSample<int> innerSample;

		public GenericSampleOfIntExplicitExposer(IGenericSample<int> sample)
		{
			this.innerSample = sample;
		}
	}
}
