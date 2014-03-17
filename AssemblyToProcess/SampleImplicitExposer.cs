using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class SampleImplicitExposer
	{
		[ExposeInterfaceImplicitly]
		private ISample innerSample;

		public SampleImplicitExposer(ISample sample)
		{
			this.innerSample = sample;
		}
	}
}
