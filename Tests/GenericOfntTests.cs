using System;
using System.Reflection;
using NUnit.Framework;

public partial class IntegrationTests
{
	[Test, Category("Generic"), Category("GenericOfInt"), Category("Members"), Category("DoesNotImplement")]
	public void GenericSampleOfIntMembersExposer_Should_Not_Implement_IGenericSampleOfInt()
	{
		var genericMembersExposerOfIntType = assembly.GetType("AssemblyToProcess.GenericSampleOfIntMembersExposer");
		var iGenericSampleOfIntType = assembly.GetType("AssemblyToProcess.IGenericSample`1").MakeGenericType(typeof(int));
		CollectionAssert.DoesNotContain(genericMembersExposerOfIntType.GetInterfaces(), iGenericSampleOfIntType);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Implicit"), Category("Implements")]
	public void GenericSampleOfIntImplicitExposer_Should_Implement_IGenericSampleOfInt()
	{
		var genericImplicitExposerOfIntType = assembly.GetType("AssemblyToProcess.GenericSampleOfIntImplicitExposer");
		var iGenericSampleOfIntType = assembly.GetType("AssemblyToProcess.IGenericSample`1").MakeGenericType(typeof(int));
		Assert.Contains(iGenericSampleOfIntType, genericImplicitExposerOfIntType.GetInterfaces());
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Explicit"), Category("Implements")]
	public void GenericSampleOfIntExplicitExposer_Should_Implement_IGenericSampleOfInt()
	{
		var genericExplicitExposerOfIntType = assembly.GetType("AssemblyToProcess.GenericSampleOfIntExplicitExposer");
		var iGenericSampleOfIntType = assembly.GetType("AssemblyToProcess.IGenericSample`1").MakeGenericType(typeof(int));
		Assert.Contains(iGenericSampleOfIntType, genericExplicitExposerOfIntType.GetInterfaces());
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Members"), Category("Method")]
	public void GenericSampleOfIntMembersExposer_Func_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		var x = 2;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		dynamic z1 = impl.Func(x, y);
		dynamic genericMembersExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntMembersExposer"), impl);
		dynamic z2 = genericMembersExposerOfInt.Func(x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Implicit"), Category("Method")]
	public void GenericSampleOfIntImplicitExposer_Func_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		var x = 2;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		dynamic z1 = impl.Func(x, y);
		dynamic genericImplicitExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntImplicitExposer"), impl);
		dynamic z2 = genericImplicitExposerOfInt.Func(x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Explicit"), Category("Method")]
	public void GenericSampleOfIntExplicitExposer_Func_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		var x = 2;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		dynamic z1 = impl.Func(x, y);

		dynamic genericExplicitExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntExplicitExposer"), impl);
		dynamic z2 = InvokeExplicitFunc(genericExplicitExposerOfInt, "IGenericSample`1",  "Func", x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Members"), Category("Property")]
	public void GenericSampleOfIntMembersExposer_ReadWriteValue_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		impl.ReadWriteValue = int.MaxValue;
		Assert.AreEqual(int.MaxValue, impl.ReadWriteValue);
		dynamic genericMembersExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntMembersExposer"), impl);
		Assert.AreEqual(impl.ReadWriteValue, genericMembersExposerOfInt.ReadWriteValue);

		genericMembersExposerOfInt.ReadWriteValue = int.MinValue;
		Assert.AreEqual(int.MinValue, genericMembersExposerOfInt.ReadWriteValue);
		Assert.AreEqual(genericMembersExposerOfInt.ReadWriteValue, impl.ReadWriteValue);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Implicit"), Category("Property")]
	public void GenericSampleOfIntImplicitExposer_ReadWriteValue_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		impl.ReadWriteValue = int.MaxValue;
		Assert.AreEqual(int.MaxValue, impl.ReadWriteValue);

		dynamic genericImplicitExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntImplicitExposer"), impl);
		Assert.AreEqual(impl.ReadWriteValue, genericImplicitExposerOfInt.ReadWriteValue);

		genericImplicitExposerOfInt.ReadWriteValue = int.MinValue;
		Assert.AreEqual(int.MinValue, genericImplicitExposerOfInt.ReadWriteValue);
		Assert.AreEqual(genericImplicitExposerOfInt.ReadWriteValue, impl.ReadWriteValue);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Explicit"), Category("Property")]
	public void GenericSampleOfIntExplicitExposer_ReadWriteValue_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		impl.ReadWriteValue = int.MaxValue;
		Assert.AreEqual(int.MaxValue, impl.ReadWriteValue);

		dynamic genericExplicitExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntExplicitExposer"), impl);
		Assert.AreEqual(impl.ReadWriteValue, GetExplicitPropertyValue(genericExplicitExposerOfInt, "IGenericSample`1", "ReadWriteValue"));

		SetExplicitPropertyValue(genericExplicitExposerOfInt, "IGenericSample`1", "ReadWriteValue", int.MinValue);
		Assert.AreEqual(int.MinValue, GetExplicitPropertyValue(genericExplicitExposerOfInt, "IGenericSample`1", "ReadWriteValue"));
		Assert.AreEqual(GetExplicitPropertyValue(genericExplicitExposerOfInt, "IGenericSample`1", "ReadWriteValue"), impl.ReadWriteValue);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Members"), Category("Indexer")]
	public void GenericSampleOfIntMembersExposer_Indexer_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		impl[int.MaxValue] = int.MaxValue;
		Assert.AreEqual(int.MaxValue, impl[int.MaxValue]);

		dynamic genericMembersExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntMembersExposer"), impl);
		Assert.AreEqual(impl[int.MaxValue], genericMembersExposerOfInt[int.MaxValue]);

		genericMembersExposerOfInt[int.MinValue] = int.MinValue;
		Assert.AreEqual(int.MinValue, genericMembersExposerOfInt[int.MinValue]);
		Assert.AreEqual(genericMembersExposerOfInt[int.MinValue], impl[int.MinValue]);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Implicit"), Category("Indexer")]
	public void GenericSampleOfIntImplicitExposer_Indexer_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		impl[int.MaxValue] = int.MaxValue;
		Assert.AreEqual(int.MaxValue, impl[int.MaxValue]);

		dynamic genericImplicitExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntImplicitExposer"), impl);
		Assert.AreEqual(impl[int.MaxValue], genericImplicitExposerOfInt[int.MaxValue]);

		genericImplicitExposerOfInt[int.MinValue] = int.MinValue;
		Assert.AreEqual(int.MinValue, genericImplicitExposerOfInt[int.MinValue]);
		Assert.AreEqual(genericImplicitExposerOfInt[int.MinValue], impl[int.MinValue]);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Explicit"), Category("Indexer")]
	public void GenericSampleOfIntExplicitExposer_Indexer_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		impl[int.MaxValue] = int.MaxValue;
		Assert.AreEqual(int.MaxValue, impl[int.MaxValue]);

		dynamic genericExplicitExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntExplicitExposer"), impl);
		Assert.AreEqual(impl[int.MaxValue], GetExplicitIndexerValue(genericExplicitExposerOfInt, "IGenericSample`1", int.MaxValue));

		SetExplicitIndexerValue(genericExplicitExposerOfInt, "IGenericSample`1", int.MinValue, int.MinValue);
		Assert.AreEqual(int.MinValue, GetExplicitIndexerValue(genericExplicitExposerOfInt, "IGenericSample`1", int.MinValue));
		Assert.AreEqual(GetExplicitIndexerValue(genericExplicitExposerOfInt, "IGenericSample`1", int.MinValue), impl[int.MinValue]);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Members"), Category("GenericMethod")]
	public void GenericSampleOfIntMembersExposer_FuncOfU_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		var x = 2;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		dynamic z1 = impl.Func<int>(x, y);
		dynamic genericMembersExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntMembersExposer"), impl);
		dynamic z2 = genericMembersExposerOfInt.Func<int>(x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Implicit"), Category("GenericMethod")]
	public void GenericSampleOfIntImplicitExposer_FuncOfU_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		var x = 2;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		dynamic z1 = impl.Func<int>(x, y);
		dynamic genericImplicitExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntImplicitExposer"), impl);
		dynamic z2 = genericImplicitExposerOfInt.Func<int>(x, y);
		Assert.AreEqual(z1, z2);
	}

	[Test, Category("Generic"), Category("GenericOfInt"), Category("Explicit"), Category("GenericMethod")]
	public void GenericSampleOfIntExplicitExposer_FuncOfU_Should_Return_Same_Value_As_That_Of_InnerSampleOfInt()
	{
		var x = 2;
		var y = 3;
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		dynamic z1 = impl.Func<int>(x, y);

		dynamic genericExplicitExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntExplicitExposer"), impl);
		dynamic z2 = InvokeExplicitGenericFunc(genericExplicitExposerOfInt, "IGenericSample`1", "Func", new Type[] { typeof(int) }, x, y);
		Assert.AreEqual(z1, z2);
	}

	public int GenericSampleOfIntMembersExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val = 0;
	[Test, Category("Generic"), Category("GenericOfInt"), Category("Members"), Category("Event")]
	public void GenericSampleOfIntMembersExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt()
	{
		var eaType = assembly.GetType("AssemblyToProcess.DummyEventArgs`1").MakeGenericType(typeof(int));
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		dynamic genericMembersExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntMembersExposer"), impl);
		EventInfo ei = genericMembersExposerOfInt.GetType().GetEvent("Event");
		Delegate handler = GetEventHandler(ei, eaType, "GenericSampleOfIntMembersExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val");

		ei.AddEventHandler(genericMembersExposerOfInt, handler);
		impl.RaiseEvent(5);
		Assert.AreEqual(GenericSampleOfIntMembersExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val, 5);

		ei.RemoveEventHandler(genericMembersExposerOfInt, handler);
		impl.RaiseEvent(10);
		Assert.AreEqual(GenericSampleOfIntMembersExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val, 5);
	}

	public int GenericSampleOfIntImplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val = 0;
	[Test, Category("Generic"), Category("GenericOfInt"), Category("Implicit"), Category("Event")]
	public void GenericSampleOfIntImplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt()
	{
		var eaType = assembly.GetType("AssemblyToProcess.DummyEventArgs`1").MakeGenericType(typeof(int));
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		dynamic genericImplicitExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntImplicitExposer"), impl);
		EventInfo ei = genericImplicitExposerOfInt.GetType().GetEvent("Event");
		Delegate handler = GetEventHandler(ei, eaType, "GenericSampleOfIntImplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val");

		ei.AddEventHandler(genericImplicitExposerOfInt, handler);
		impl.RaiseEvent(5);
		Assert.AreEqual(5, GenericSampleOfIntImplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val);

		ei.RemoveEventHandler(genericImplicitExposerOfInt, handler);
		impl.RaiseEvent(10);
		Assert.AreEqual(5, GenericSampleOfIntImplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val);
	}

	public int GenericSampleOfIntExplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val = 0;
	[Test, Category("Generic"), Category("GenericOfInt"), Category("Explicit"), Category("Event")]
	public void GenericSampleOfIntExplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt()
	{
		var eaType = assembly.GetType("AssemblyToProcess.DummyEventArgs`1").MakeGenericType(typeof(int));
		dynamic impl = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleImpl`1").MakeGenericType(typeof(int)));
		dynamic genericExplicitExposerOfInt = Activator.CreateInstance(assembly.GetType("AssemblyToProcess.GenericSampleOfIntExplicitExposer"), impl);
		EventInfo ei = genericExplicitExposerOfInt.GetType().GetInterface("IGenericSample`1").GetEvent("Event");
		Delegate handler = GetEventHandler(ei, eaType, "GenericSampleOfIntExplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val");

		ei.AddEventHandler(genericExplicitExposerOfInt, handler);
		impl.RaiseEvent(5);
		Assert.AreEqual(5, GenericSampleOfIntExplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val);

		ei.RemoveEventHandler(genericExplicitExposerOfInt, handler);
		impl.RaiseEvent(10);
		Assert.AreEqual(5, GenericSampleOfIntExplicitExposer_Event_Should_Invoke_That_Of_InnerSampleOfInt_Val);
	}

}