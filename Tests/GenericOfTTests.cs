using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;

public partial class IntegrationTests
{
	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("DoesNotImplement")]
	public void GenericSampleOfTMembersExposer_Should_Not_Implement_IGenericSampleOfT()
	{
		var genericMembersExposerOfTType = assembly.GetType("AssemblyToProcess.GenericSampleOfTMembersExposer`1").MakeGenericType(typeof(double));
		var iGenericSampleOfTType = assembly.GetType("AssemblyToProcess.IGenericSample`1").MakeGenericType(typeof(double));
		CollectionAssert.DoesNotContain(genericMembersExposerOfTType.GetInterfaces(), iGenericSampleOfTType);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Implicit"), Category("Implements")]
	public void GenericSampleOfTImplicitExposer_Should_Implement_IGenericSampleOfT()
	{
		var genericImplicitExposerOfTType = assembly.GetType("AssemblyToProcess.GenericSampleOfTImplicitExposer`1").MakeGenericType(typeof(double));
		var iGenericSampleOfTType = assembly.GetType("AssemblyToProcess.IGenericSample`1").MakeGenericType(typeof(double));
		Assert.Contains(iGenericSampleOfTType, genericImplicitExposerOfTType.GetInterfaces());
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Explicit"), Category("Implements")]
	public void GenericSampleOfTExplicitExposer_Should_Implement_IGenericSampleOfT()
	{
		var genericExplicitExposerOfTType = assembly.GetType("AssemblyToProcess.GenericSampleOfTExplicitExposer`1").MakeGenericType(typeof(double));
		var iGenericSampleOfTType = assembly.GetType("AssemblyToProcess.IGenericSample`1").MakeGenericType(typeof(double));
		Assert.Contains(iGenericSampleOfTType, genericExplicitExposerOfTType.GetInterfaces());
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("Method")]
	public void GenericSampleOfTMembersExposer_Func_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		var x = 2.0;
		var y = 3.0;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		dynamic z1 = impl.Func(x, y);
		dynamic genericMembersExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTMembersExposer`1").MakeGenericType(typeof(double)), impl);
		dynamic z2 = genericMembersExposerOfT.Func(x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Implicit"), Category("Method")]
	public void GenericSampleOfTImplicitExposer_Func_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		var x = 2.0;
		var y = 3.0;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		dynamic z1 = impl.Func(x, y);
		dynamic genericImplicitExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTImplicitExposer`1").MakeGenericType(typeof(double)), impl);
		dynamic z2 = genericImplicitExposerOfT.Func(x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Explicit"), Category("Method")]
	public void GenericSampleOfTExplicitExposer_Func_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		var x = 2.0;
		var y = 3.0;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		dynamic z1 = impl.Func(x, y);

		dynamic genericExplicitExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTExplicitExposer`1").MakeGenericType(typeof(double)), impl);
		dynamic z2 = InvokeExplicitFunc(genericExplicitExposerOfT, "IGenericSample`1", "Func", x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("Method"), Category("HandCoded")]
	public void HandCodedGenericSampleOfTMembersExposer_Func_Should_Not_Be_Automatically_Generated()
	{
		var membersExposerType = assembly.GetType("AssemblyToProcess.HandCodedGenericSampleOfTMembersExposer`1");
		var method = membersExposerType.GetMethods().FirstOrDefault(m => m.Name == "Func" && !m.IsGenericMethod);
		Assert.IsEmpty(method.GetCustomAttributes(typeof(GeneratedCodeAttribute), false));
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("Property")]
	public void GenericSampleOfTMembersExposer_ReadWriteValue_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		impl.ReadWriteValue = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl.ReadWriteValue);
		dynamic genericMembersExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTMembersExposer`1").MakeGenericType(typeof(double)), impl);
		Assert.AreEqual(impl.ReadWriteValue, genericMembersExposerOfT.ReadWriteValue);

		genericMembersExposerOfT.ReadWriteValue = double.MinValue;
		Assert.AreEqual(double.MinValue, genericMembersExposerOfT.ReadWriteValue);
		Assert.AreEqual(genericMembersExposerOfT.ReadWriteValue, impl.ReadWriteValue);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Implicit"), Category("Property")]
	public void GenericSampleOfTImplicitExposer_ReadWriteValue_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		impl.ReadWriteValue = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl.ReadWriteValue);

		dynamic genericImplicitExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTImplicitExposer`1").MakeGenericType(typeof(double)), impl);
		Assert.AreEqual(impl.ReadWriteValue, genericImplicitExposerOfT.ReadWriteValue);

		genericImplicitExposerOfT.ReadWriteValue = double.MinValue;
		Assert.AreEqual(double.MinValue, genericImplicitExposerOfT.ReadWriteValue);
		Assert.AreEqual(genericImplicitExposerOfT.ReadWriteValue, impl.ReadWriteValue);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Explicit"), Category("Property")]
	public void GenericSampleOfTExplicitExposer_ReadWriteValue_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		impl.ReadWriteValue = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl.ReadWriteValue);

		dynamic genericExplicitExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTExplicitExposer`1").MakeGenericType(typeof(double)), impl);
		Assert.AreEqual(impl.ReadWriteValue, GetExplicitPropertyValue(genericExplicitExposerOfT, "IGenericSample`1", "ReadWriteValue"));

		SetExplicitPropertyValue(genericExplicitExposerOfT, "IGenericSample`1", "ReadWriteValue", double.MinValue);
		Assert.AreEqual(double.MinValue, GetExplicitPropertyValue(genericExplicitExposerOfT, "IGenericSample`1", "ReadWriteValue"));
		Assert.AreEqual(GetExplicitPropertyValue(genericExplicitExposerOfT, "IGenericSample`1", "ReadWriteValue"), impl.ReadWriteValue);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("Property"), Category("HandCoded")]
	public void HandCodedGenericSampleOfTMembersExposer_ReadWriteValue_Should_Not_Be_Automatically_Generated()
	{
		var membersExposerType = assembly.GetType("AssemblyToProcess.HandCodedGenericSampleOfTMembersExposer`1");
		var property = membersExposerType.GetProperty("ReadWriteValue");
		Assert.IsEmpty(property.GetCustomAttributes(typeof(GeneratedCodeAttribute), false));
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("Indexer")]
	public void GenericSampleOfTMembersExposer_Indexer_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		impl[double.MaxValue] = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl[double.MaxValue]);

		dynamic genericMembersExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTMembersExposer`1").MakeGenericType(typeof(double)), impl);
		Assert.AreEqual(impl[double.MaxValue], genericMembersExposerOfT[double.MaxValue]);

		genericMembersExposerOfT[double.MinValue] = double.MinValue;
		Assert.AreEqual(double.MinValue, genericMembersExposerOfT[double.MinValue]);
		Assert.AreEqual(genericMembersExposerOfT[double.MinValue], impl[double.MinValue]);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Implicit"), Category("Indexer")]
	public void GenericSampleOfTImplicitExposer_Indexer_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		impl[double.MaxValue] = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl[double.MaxValue]);

		dynamic genericImplicitExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTImplicitExposer`1").MakeGenericType(typeof(double)), impl);
		Assert.AreEqual(impl[double.MaxValue], genericImplicitExposerOfT[double.MaxValue]);

		genericImplicitExposerOfT[double.MinValue] = double.MinValue;
		Assert.AreEqual(double.MinValue, genericImplicitExposerOfT[double.MinValue]);
		Assert.AreEqual(genericImplicitExposerOfT[double.MinValue], impl[double.MinValue]);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Explicit"), Category("Indexer")]
	public void GenericSampleOfTExplicitExposer_Indexer_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		impl[double.MaxValue] = double.MaxValue;
		Assert.AreEqual(double.MaxValue, impl[double.MaxValue]);

		dynamic genericExplicitExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTExplicitExposer`1").MakeGenericType(typeof(double)), impl);
		Assert.AreEqual(impl[double.MaxValue], GetExplicitIndexerValue(genericExplicitExposerOfT, "IGenericSample`1", double.MaxValue));

		SetExplicitIndexerValue(genericExplicitExposerOfT, "IGenericSample`1", double.MinValue, double.MinValue);
		Assert.AreEqual(double.MinValue, GetExplicitIndexerValue(genericExplicitExposerOfT, "IGenericSample`1", double.MinValue));
		Assert.AreEqual(GetExplicitIndexerValue(genericExplicitExposerOfT, "IGenericSample`1", double.MinValue), impl[double.MinValue]);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("Indexer"), Category("HandCoded")]
	public void HandCodedGenericSampleOfTMembersExposer_Indexer_Should_Not_Be_Automatically_Generated()
	{
		var membersExposerType = assembly.GetType("AssemblyToProcess.HandCodedGenericSampleOfTMembersExposer`1");
		var property = membersExposerType.GetProperty("Item");
		Assert.IsEmpty(property.GetCustomAttributes(typeof(GeneratedCodeAttribute), false));
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("GenericMethod")]
	public void GenericSampleOfTMembersExposer_FuncOfU_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		var x = 2.0;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		dynamic z1 = impl.Func<int>(x, y);
		dynamic genericMembersExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTMembersExposer`1").MakeGenericType(typeof(double)), impl);
		dynamic z2 = genericMembersExposerOfT.Func<int>(x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Implicit"), Category("GenericMethod")]
	public void GenericSampleOfTImplicitExposer_FuncOfU_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		var x = 2.0;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		dynamic z1 = impl.Func<int>(x, y);
		dynamic genericImplicitExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTImplicitExposer`1").MakeGenericType(typeof(double)), impl);
		dynamic z2 = genericImplicitExposerOfT.Func<int>(x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Explicit"), Category("GenericMethod")]
	public void GenericSampleOfTExplicitExposer_FuncOfU_Should_Return_Same_Value_As_That_Of_InnerSampleOfT()
	{
		var x = 2.0;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		dynamic z1 = impl.Func<int>(x, y);

		dynamic genericExplicitExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTExplicitExposer`1").MakeGenericType(typeof(double)), impl);
		dynamic z2 = InvokeExplicitGenericFunc(genericExplicitExposerOfT, "IGenericSample`1", "Func", new Type[] { typeof(int) }, x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("Method"), Category("HandCoded")]
	public void HandCodedGenericSampleOfTMembersExposer_FuncOfU_Should_Not_Be_Automatically_Generated()
	{
		var membersExposerType = assembly.GetType("AssemblyToProcess.HandCodedGenericSampleOfTMembersExposer`1");
		var method = membersExposerType.GetMethods().FirstOrDefault(m => m.Name == "Func" && m.IsGenericMethod);
		Assert.IsEmpty(method.GetCustomAttributes(typeof(GeneratedCodeAttribute), false));
	}

	public double GenericSampleOfTMembersExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val = 0.0;
	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("Event")]
	public void GenericSampleOfTMembersExposer_Event_Should_Invoke_That_Of_InnerSampleOfT()
	{
		var eaType = assembly.GetType("AssemblyToProcess.DummyEventArgs`1").MakeGenericType(typeof(double));
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		dynamic genericMembersExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTMembersExposer`1").MakeGenericType(typeof(double)), impl);
		EventInfo ei = genericMembersExposerOfT.GetType().GetEvent("Event");
		Delegate handler = GetEventHandler(ei, eaType, "GenericSampleOfTMembersExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val");

		ei.AddEventHandler(genericMembersExposerOfT, handler);
		impl.RaiseEvent(5.0);
		Assert.AreEqual(5.0, GenericSampleOfTMembersExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val);

		ei.RemoveEventHandler(genericMembersExposerOfT, handler);
		impl.RaiseEvent(10.0);
		Assert.AreEqual(5.0, GenericSampleOfTMembersExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val);
	}

	public double GenericSampleOfTImplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val = 0.0;
	[Test, Category("Generic"), Category("GenericOfT"), Category("Implicit"), Category("Event")]
	public void GenericSampleOfTImplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfT()
	{
		var eaType = assembly.GetType("AssemblyToProcess.DummyEventArgs`1").MakeGenericType(typeof(double));
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		dynamic genericImplicitExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTImplicitExposer`1").MakeGenericType(typeof(double)), impl);
		EventInfo ei = genericImplicitExposerOfT.GetType().GetEvent("Event");
		Delegate handler = GetEventHandler(ei, eaType, "GenericSampleOfTImplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val");

		ei.AddEventHandler(genericImplicitExposerOfT, handler);
		impl.RaiseEvent(5.0);
		Assert.AreEqual(5.0, GenericSampleOfTImplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val);

		ei.RemoveEventHandler(genericImplicitExposerOfT, handler);
		impl.RaiseEvent(10.0);
		Assert.AreEqual(5.0, GenericSampleOfTImplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val);
	}

	public double GenericSampleOfTExplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val = 0.0;
	[Test, Category("Generic"), Category("GenericOfT"), Category("Explicit"), Category("Event")]
	public void GenericSampleOfTExplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfT()
	{
		var eaType = assembly.GetType("AssemblyToProcess.DummyEventArgs`1").MakeGenericType(typeof(double));
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(double)));
		dynamic genericExplicitExposerOfT = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfTExplicitExposer`1").MakeGenericType(typeof(double)), impl);
		EventInfo ei = genericExplicitExposerOfT.GetType().GetInterface("IGenericSample`1").GetEvent("Event");
		Delegate handler = GetEventHandler(ei, eaType, "GenericSampleOfTExplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val");

		ei.AddEventHandler(genericExplicitExposerOfT, handler);
		impl.RaiseEvent(5.0);
		Assert.AreEqual(5.0, GenericSampleOfTExplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val);

		ei.RemoveEventHandler(genericExplicitExposerOfT, handler);
		impl.RaiseEvent(10.0);
		Assert.AreEqual(5.0, GenericSampleOfTExplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfT_Val);
	}

	[Test, Category("Generic"), Category("GenericOfT"), Category("Members"), Category("Event"), Category("HandCoded")]
	public void HandCodedGenericSampleOfTMembersExposer_Event_Should_Not_Be_Automatically_Generated()
	{
		var membersExposerType = assembly.GetType("AssemblyToProcess.HandCodedGenericSampleOfTMembersExposer`1");
		var @event = membersExposerType.GetEvent("Event");
		Assert.IsEmpty(@event.GetCustomAttributes(typeof(GeneratedCodeAttribute), false));
	}

}