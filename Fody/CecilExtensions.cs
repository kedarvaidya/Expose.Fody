using System;
using System.Collections.Generic;
using System.Linq;
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

	internal static IEnumerable<MethodDefinition> Public(this IEnumerable<MethodDefinition> self)
	{
		return self.Where(m => m.IsPublic);
	}

	internal static IEnumerable<MethodDefinition> NonSpecial(this IEnumerable<MethodDefinition> self)
	{
		return self.Where(m => !m.Attributes.HasFlag(MethodAttributes.SpecialName));
	}
}

