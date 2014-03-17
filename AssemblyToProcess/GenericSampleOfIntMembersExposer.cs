using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfIntMembersExposer
	{
		[ExposeMembers]
		private IGenericSample<int> innerSample;

		public GenericSampleOfIntMembersExposer(IGenericSample<int> sample)
		{
			innerSample = sample;
		}
	}
}
