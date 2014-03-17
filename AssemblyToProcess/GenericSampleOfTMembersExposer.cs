using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
	public class GenericSampleOfTMembersExposer<T>
	{
		[ExposeMembers]
		private IGenericSample<T> innerSample;

		public GenericSampleOfTMembersExposer(IGenericSample<T> sample)
		{
			innerSample = sample;
		}
	}
}
