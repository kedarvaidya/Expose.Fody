using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public partial class IntegrationTests
{
	public object InvokeExplicitFunc(object instance, string @interface, string method, params object[] args)
	{
		return instance.GetType().GetInterface(@interface).InvokeMember(method, BindingFlags.InvokeMethod, null, instance, args);
	}

	public void InvokeExplicitAction(object instance, string @interface, string method, params object[] args)
	{
		instance.GetType().GetInterface(@interface).InvokeMember(method, BindingFlags.InvokeMethod, null, instance, args);
	}

	public object GetExplicitPropertyValue(object instance, string @interface, string property)
	{
		return InvokeExplicitFunc(instance, @interface, "get_" + property);
	}

	public void SetExplicitPropertyValue(object instance, string @interface, string property, object value)
	{
		InvokeExplicitAction(instance, @interface, "set_" + property, value);
	}

	public object GetExplicitIndexerValue(object instance, string @interface, object key)
	{
		return InvokeExplicitFunc(instance, @interface, "get_Item", key);
	}

	public void SetExplicitIndexerValue(object instance, string @interface, object key, object value)
	{
		InvokeExplicitFunc(instance, @interface, "set_Item", key, value);
	}
}
