using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public partial class IntegrationTests
{
	[Test, Category("NonGeneric"), Category("Members"), Category("DoesNotImplement")]
	public void SampleMembersExposer_Should_Not_Implement_ISample()
	{
		var membersExposerType = assembly.GetType("AssemblyToProcess.SampleMembersExposer");
		var iSampleType = assembly.GetType("AssemblyToProcess.ISample");
		CollectionAssert.DoesNotContain(membersExposerType.GetInterfaces(), iSampleType);
	}

	[Test, Category("NonGeneric"), Category("Implicit"), Category("Implements")]
	public void SampleImplicitExposer_Should_Implement_ISample()
	{
		var implicitExposerType = assembly.GetType("AssemblyToProcess.SampleImplicitExposer");
		var iSampleType = assembly.GetType("AssemblyToProcess.ISample");
		Assert.Contains(iSampleType, implicitExposerType.GetInterfaces());
	}

	[Test, Category("NonGeneric"), Category("Explicit"), Category("Implements")]
	public void SampleExplicitExposer_Should_Implement_ISample()
	{
		var explicitExposerType = assembly.GetType("AssemblyToProcess.SampleExplicitExposer");
		var iSampleType = assembly.GetType("AssemblyToProcess.ISample");
		Assert.Contains(iSampleType, explicitExposerType.GetInterfaces());
	}

	[Test, Category("NonGeneric"), Category("Members"), Category("Method")]
	public void SampleMembersExposer_Func_Should_Return_Same_Value_As_That_Of_InnerSample()
	{
		var x = 2;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImpl"));
		dynamic z1 = impl.Func(x, y);
		dynamic membersExposer = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleMembersExposer"), impl);
		dynamic z2 = membersExposer.Func(x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("NonGeneric"), Category("Implicit"), Category("Method")]
	public void SampleImplicitExposer_Func_Should_Return_Same_Value_As_That_Of_InnerSample()
	{
		var x = 2;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImpl"));
		dynamic z1 = impl.Func(x, y);
		dynamic implicitExposer = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImplicitExposer"), impl);
		dynamic z2 = implicitExposer.Func(x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("NonGeneric"), Category("Explicit"), Category("Method")]
	public void SampleExplicitExposer_Func_Should_Return_Same_Value_As_That_Of_InnerSample()
	{
		var x = 2;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImpl"));
		dynamic z1 = impl.Func(x, y);
		object explicitExposer = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleExplicitExposer"), (object)impl);
		dynamic z2 = InvokeExplicitFunc(explicitExposer, "ISample", "Func", x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("NonGeneric"), Category("Members"), Category("Method"), Category("HandCoded")]
	public void HandCodedSampleMembersExposer_Func_Should_Not_Be_Automatically_Generated()
	{
		var membersExposerType = assembly.GetType("AssemblyToProcess.HandCodedSampleMembersExposer");
		var method = membersExposerType.GetMethod("Func");
		Assert.IsEmpty(method.GetCustomAttributes(typeof(GeneratedCodeAttribute), false));
	}

	[Test, Category("NonGeneric"), Category("Members"), Category("Property")]
	public void SampleMembersExposer_ReadWriteValue_Should_Return_Same_Value_As_That_Of_InnerSample()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImpl"));
		impl.ReadWriteValue = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl.ReadWriteValue);

		dynamic membersExposer = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleMembersExposer"), impl);
		Assert.AreEqual(impl.ReadWriteValue, membersExposer.ReadWriteValue);

		membersExposer.ReadWriteValue = double.MinValue;
		Assert.AreEqual(double.MinValue, membersExposer.ReadWriteValue);
		Assert.AreEqual(membersExposer.ReadWriteValue, impl.ReadWriteValue);
	}

	[Test, Category("NonGeneric"), Category("Implicit"), Category("Property")]
	public void SampleImplicitExposer_ReadWriteValue_Should_Return_Same_Value_As_That_Of_InnerSample()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImpl"));
		impl.ReadWriteValue = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl.ReadWriteValue);

		dynamic implicitExposer = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImplicitExposer"), impl);
		Assert.AreEqual(impl.ReadWriteValue, implicitExposer.ReadWriteValue);

		implicitExposer.ReadWriteValue = double.MinValue;
		Assert.AreEqual(double.MinValue, implicitExposer.ReadWriteValue);
		Assert.AreEqual(implicitExposer.ReadWriteValue, impl.ReadWriteValue);
	}

	[Test, Category("NonGeneric"), Category("Explicit"), Category("Property")]
	public void SampleExplicitExposer_ReadWriteValue_Should_Return_Same_Value_As_That_Of_InnerSample()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImpl"));
		impl.ReadWriteValue = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl.ReadWriteValue);

		object explicitExposer = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleExplicitExposer"), (object)impl);
		Assert.AreEqual(impl.ReadWriteValue, GetExplicitPropertyValue(explicitExposer, "ISample", "ReadWriteValue"));

		SetExplicitPropertyValue(explicitExposer, "ISample", "ReadWriteValue", double.MinValue);
		Assert.AreEqual(double.MinValue, GetExplicitPropertyValue(explicitExposer, "ISample", "ReadWriteValue"));
		Assert.AreEqual(GetExplicitPropertyValue(explicitExposer, "ISample", "ReadWriteValue"), impl.ReadWriteValue);
	}

	[Test, Category("NonGeneric"), Category("Members"), Category("Property"), Category("HandCoded")]
	public void HandCodedSampleMembersExposer_ReadWriteValue_Should_Not_Be_Automatically_Generated()
	{
		var membersExposerType = assembly.GetType("AssemblyToProcess.HandCodedSampleMembersExposer");
		var property = membersExposerType.GetProperty("ReadWriteValue");
		Assert.IsEmpty(property.GetCustomAttributes(typeof(GeneratedCodeAttribute), false));
	}

	[Test, Category("NonGeneric"), Category("Members"), Category("Indexer")]
	public void SampleMembersExposer_Indexer_Should_Return_Same_Value_As_That_Of_InnerSample()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImpl"));
		impl[double.MaxValue] = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl[double.MaxValue]);
		dynamic membersExposer = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleMembersExposer"), impl);
		Assert.AreEqual(impl[double.MaxValue], membersExposer[double.MaxValue]);

		membersExposer[double.MinValue] = double.MinValue;
		Assert.AreEqual(double.MinValue, membersExposer[double.MinValue]);
		Assert.AreEqual(membersExposer[double.MinValue], impl[double.MinValue]);
	}

	[Test, Category("NonGeneric"), Category("Implicit"), Category("Indexer")]
	public void SampleImplicitExposer_Indexer_Should_Return_Same_Value_As_That_Of_InnerSample()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImpl"));
		impl[double.MaxValue] = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl[double.MaxValue]);
		dynamic implicitExposer = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImplicitExposer"), impl);
		Assert.AreEqual(impl[double.MaxValue], implicitExposer[double.MaxValue]);

		implicitExposer[double.MinValue] = double.MinValue;
		Assert.AreEqual(double.MinValue, implicitExposer[double.MinValue]);
		Assert.AreEqual(implicitExposer[double.MinValue], impl[double.MinValue]);
	}

	[Test, Category("NonGeneric"), Category("Explicit"), Category("Indexer")]
	public void SampleExplicitExposer_Indexer_Should_Return_Same_Value_As_That_Of_InnerSample()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleImpl"));
		impl[double.MaxValue] = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl[double.MaxValue]);

		object explicitExposer = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.SampleExplicitExposer"), (object)impl);
		Assert.AreEqual(impl[double.MaxValue], GetExplicitIndexerValue(explicitExposer, "ISample", double.MaxValue));

		SetExplicitIndexerValue(explicitExposer, "ISample", double.MinValue, double.MinValue);
		Assert.AreEqual(double.MinValue, GetExplicitIndexerValue(explicitExposer, "ISample", double.MinValue));
		Assert.AreEqual(GetExplicitIndexerValue(explicitExposer, "ISample", double.MinValue), impl[double.MinValue]);
	}

	[Test, Category("NonGeneric"), Category("Members"), Category("Indexer"), Category("HandCoded")]
	public void HandCodedSampleMembersExposer_Indexer_Should_Not_Be_Automatically_Generated()
	{
		var membersExposerType = assembly.GetType("AssemblyToProcess.HandCodedSampleMembersExposer");
		var property = membersExposerType.GetProperty("Item");
		Assert.IsEmpty(property.GetCustomAttributes(typeof(GeneratedCodeAttribute), false));
	}

}
