using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

	// Does a very basic check to determine whether method is generic
	public object InvokeExplicitGenericFunc(object instance, string @interface, string method, Type[] genArgs, params object[] args)
	{
		var type = instance.GetType().GetInterface(@interface);
		var genMethod = type.GetMethods().FirstOrDefault(m => m.Name == method && m.ContainsGenericParameters).MakeGenericMethod(genArgs);
		return genMethod.Invoke(instance, args);
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

	private Delegate GetEventHandler(EventInfo ei, Type eaType, string fieldToAssign)
	{
		ParameterExpression senderParam = Expression.Parameter(typeof(object), "sender");
		ParameterExpression eventArgsParam = Expression.Parameter(eaType, "e");
		Delegate handler = Expression.Lambda(ei.EventHandlerType,
			Expression.Assign(
				Expression.Field(Expression.Constant(this), this.GetType().GetField(fieldToAssign)),
				Expression.Property(eventArgsParam, eaType.GetProperty("Value").GetGetMethod())
			),
			senderParam,
			eventArgsParam
		).Compile();
		return handler;
	}
}
