using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class SampleExplicitExposer
	{
		[ExposeInterfaceExplicitly]
		private ISample innerSample;

		public SampleExplicitExposer(ISample sample)
		{
			this.innerSample = sample;
		}
	}
}
