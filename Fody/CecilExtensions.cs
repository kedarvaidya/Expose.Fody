using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

internal static class CecilExtensions
{
	internal static IEnumerable<TypeDefinition> ClassOrStruct(this IEnumerable<TypeDefinition> self)
	{
		return self.Where(t => t.IsClass || (t.IsValueType && !t.IsEnum));
	}

	internal static IEnumerable<MemberDefinitionCustomAttributePair<TMemberDefinition>> WithAttribute<TMemberDefinition>(this IEnumerable<TMemberDefinition> self, string attributeName) where TMemberDefinition : IMemberDefinition
	{
		return self.Select(m => new MemberDefinitionCustomAttributePair<TMemberDefinition>(member: m, attribute: m.CustomAttributes.FirstOrDefault(a => a.AttributeType.Name == attributeName)))
					.Where(map => map.Attribute != null);
	}

	internal static IEnumerable<PropertyDefinition> Public(this IEnumerable<PropertyDefinition> self)
	{
		return self.Where(p => (p.GetMethod != null && p.GetMethod.IsPublic) || (p.SetMethod != null && p.SetMethod.IsPublic));
	}

	internal static IEnumerable<EventDefinition> Public(this IEnumerable<EventDefinition> self)
	{
		return self.Where(e => (e.AddMethod != null && e.AddMethod.IsPublic) || (e.RemoveMethod != null && e.RemoveMethod.IsPublic));
	}

	internal static IEnumerable<MethodDefinition> Public(this IEnumerable<MethodDefinition> self)
	{
		return self.Where(m => m.IsPublic);
	}

	internal static IEnumerable<MethodDefinition> NonSpecial(this IEnumerable<MethodDefinition> self)
	{
		return self.Where(m => !m.Attributes.HasFlag(MethodAttributes.SpecialName));
	}

	internal static GenericInstanceType MakeGenericInstanceType(this TypeReference self, params TypeReference[] arguments)
	{
		if (self == null)
			throw new ArgumentException("self");
		if (arguments == null)
			throw new ArgumentNullException("arguments");
		if (!arguments.Any())
			throw new ArgumentException("{0} cannot be empty", "arguments");
		if (self.GenericParameters.Count != arguments.Length)
			throw new ArgumentException("No of arguments do not match");

		var reference = new GenericInstanceType(self);
		foreach (var arg in arguments)
			reference.GenericArguments.Add(arg);
		return reference;
	}

	internal static TypeReference MakeGenericInstanceTypeWithGenericParametersAsGenericArguments(this TypeReference self)
	{
		return MakeGenericInstanceType(self, self.GenericParameters.ToArray());
	}

	internal static MethodReference MakeGenericInstanceMethod(this MethodReference self, TypeReference[] arguments)
	{
		if (self == null)
			throw new ArgumentException("self");
		if (arguments == null)
			throw new ArgumentNullException("arguments");
		if (!arguments.Any())
			throw new ArgumentException("{0} cannot be empty", "arguments");
		if (self.GenericParameters.Count != arguments.Length)
			throw new ArgumentException("No of arguments do not match");

		var refernce = new GenericInstanceMethod(self);
		foreach (var methodArg in arguments)
			refernce.GenericArguments.Add(methodArg);
		return refernce;
	}

	internal static MethodReference With(this MethodReference self, TypeReference declaringType = null, bool resolveGenericReturnTypeAndParameterTypes = false)
	{
		if (declaringType == null)
			return self;

		var genericDeclaringType = declaringType as GenericInstanceType;
		IDictionary<string, TypeReference> genericArgsDict = null;
		if (resolveGenericReturnTypeAndParameterTypes && genericDeclaringType != null)
			genericArgsDict = GetGenericArgumentsDictionary(self.DeclaringType, genericDeclaringType.GenericArguments);

		bool isGeneric = genericArgsDict != null && genericArgsDict.Any();

		MethodReference reference = new MethodReference(self.Name, isGeneric ? self.ReturnType.ResolveGenericType(genericArgsDict) : self.ReturnType)
		{
			DeclaringType = declaringType ?? self.DeclaringType,
			HasThis = self.HasThis,
			ExplicitThis = self.ExplicitThis,
			CallingConvention = self.CallingConvention,
		};

		foreach (var genericParameter in self.GenericParameters)
			reference.GenericParameters.Add(new GenericParameter(genericParameter.Name, reference));

		foreach (var parameter in self.Parameters)
			reference.Parameters.Add(new ParameterDefinition(parameter.Name, parameter.Attributes, isGeneric ? parameter.ParameterType.ResolveGenericType(genericArgsDict) : parameter.ParameterType));

		return reference;
	}

	internal static FieldReference With(this FieldReference self, TypeReference declaringType = null, TypeReference fieldType = null)
	{
		if (declaringType == null && fieldType == null)
			return self;

		FieldReference reference = new FieldReference(self.Name, fieldType ?? self.FieldType)
		{
			DeclaringType = declaringType ?? self.DeclaringType,
		};

		return reference;
	}

	internal static TypeReference ResolveGenericPropertyType(this PropertyReference self, IEnumerable<TypeReference> arguments)
	{
		if (self == null)
			throw new ArgumentException("self");
		if (arguments == null)
			throw new ArgumentNullException("arguments");
		if (!arguments.Any())
			throw new ArgumentException("{0} cannot be empty", "arguments");
		if (self.DeclaringType.GenericParameters.Count != arguments.Count())
			throw new ArgumentException("Number of {0} should match no of GenericParameters in DeclaringType", "arguments");

		var genericDeclaringType = self.DeclaringType.MakeGenericInstanceType(arguments.ToArray());
		var genericArgsDict = self.DeclaringType.GetGenericArgumentsDictionary(arguments);

		return self.PropertyType.ResolveGenericType(genericArgsDict);
	}

	internal static TypeReference ResolveGenericEventType(this EventReference self, IEnumerable<TypeReference> arguments)
	{
		if (self == null)
			throw new ArgumentException("self");
		if (arguments == null)
			throw new ArgumentNullException("arguments");
		if (!arguments.Any())
			throw new ArgumentException("{0} cannot be empty", "arguments");
		if (self.DeclaringType.GenericParameters.Count != arguments.Count())
			throw new ArgumentException("Number of {0} should match no of GenericParameters in DeclaringType", "arguments");

		var genericDeclaringType = self.DeclaringType.MakeGenericInstanceType(arguments.ToArray());
		var genericArgsDict = self.DeclaringType.GetGenericArgumentsDictionary(arguments);

		return self.EventType.ResolveGenericType(genericArgsDict);
	}

	private static TypeReference ResolveGenericType(this TypeReference self, IDictionary<string, TypeReference> genericArgsDict)
	{
		if (self is GenericParameter)
		{
			TypeReference result;
			return genericArgsDict.TryGetValue(self.FullName, out result) ? result : self;
		}

		var genericInstanceSelf = self as GenericInstanceType;
		if (genericInstanceSelf != null)
		{
			var result = new GenericInstanceType(genericInstanceSelf.ElementType);
			foreach (var genArg in genericInstanceSelf.GenericArguments)
			{
				TypeReference resolvedGenArg = null;
				if (genericArgsDict.TryGetValue(genArg.FullName, out resolvedGenArg))
					result.GenericArguments.Add(resolvedGenArg);
				else
					result.GenericArguments.Add(ResolveGenericType(genArg, genericArgsDict));
			}
			return result;
		}

		return self;
	}

	private static IDictionary<string, TypeReference> GetGenericArgumentsDictionary(this TypeReference self, IEnumerable<TypeReference> arguments)
	{
		return self.GenericParameters.Zip(arguments, (gp, ga) => new { gp, ga }).ToDictionary(i => i.gp.FullName, i => i.ga);
	}
}

