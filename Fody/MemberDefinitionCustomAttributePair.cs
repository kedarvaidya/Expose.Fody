using Mono.Cecil;

internal class MemberDefinitionCustomAttributePair<TMemberDefinition> where TMemberDefinition : IMemberDefinition
{
	public readonly TMemberDefinition Member;

	public readonly CustomAttribute Attribute;

	public MemberDefinitionCustomAttributePair(TMemberDefinition member, CustomAttribute attribute)
	{
		Member = member;
		Attribute = attribute;
	}
}

