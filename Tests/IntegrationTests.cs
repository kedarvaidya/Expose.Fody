using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;


[TestFixture]
public partial class IntegrationTests
{
	Assembly assembly;
	List<string> warnings = new List<string>();
	string beforeAssemblyPath;
	string afterAssemblyPath;

	public IntegrationTests()
	{
		beforeAssemblyPath = Path.GetFullPath(@"..\..\..\AssemblyToProcess\bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)

		beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif

		afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
		File.Copy(beforeAssemblyPath, afterAssemblyPath, true);
		
		var assemblyResolver = new MockAssemblyResolver
			{
				Directory = Path.GetDirectoryName(beforeAssemblyPath)
			};
		var moduleDefinition = ModuleDefinition.ReadModule(afterAssemblyPath,new ReaderParameters
			{
				AssemblyResolver = assemblyResolver
			});
		var weavingTask = new ModuleWeaver
							  {
								  ModuleDefinition = moduleDefinition,
								  AssemblyResolver = assemblyResolver,
							  };

		weavingTask.Execute();
		moduleDefinition.Write(afterAssemblyPath);

		assembly = Assembly.LoadFile(afterAssemblyPath);
	}

#if(DEBUG)
	[Test]
	public void PeVerify()
	{
		Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
	}
#endif
}