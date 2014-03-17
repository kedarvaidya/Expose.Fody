using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class SampleMembersExposer
	{
		[ExposeMembers]
		private ISample innerSample;

		public SampleMembersExposer(ISample sample)
		{
			this.innerSample = sample;
		}
	}
}
