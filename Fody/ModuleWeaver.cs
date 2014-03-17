using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class ModuleWeaver
{
	public ModuleDefinition ModuleDefinition { get; set; }
	public IAssemblyResolver AssemblyResolver { get; set; }

	public const string ExposeMembersAttribute = "ExposeMembersAttribute";
	public const string ExposeInterfaceImplicitlyAttribute = "ExposeInterfaceImplicitlyAttribute";
	public const string ExposeInterfaceExplicitlyAttribute = "ExposeInterfaceExplicitlyAttribute";

	public void Execute()
	{
		foreach (var typeToProcess in ModuleDefinition.Types.ClassOrStruct())
		{
			var exposedTypes = new Dictionary<string, MemberDefinitionCustomAttributePair<FieldDefinition>>();

			var fieldsWithExposeMembersAttribute = typeToProcess.Fields.WithAttribute<FieldDefinition>(ExposeMembersAttribute);
			foreach (var fap in fieldsWithExposeMembersAttribute)
			{
				var fieldToProcess = fap.Member;

				MemberDefinitionCustomAttributePair<FieldDefinition> alreadyExposed;
				if (exposedTypes.TryGetValue(fieldToProcess.FieldType.FullName, out alreadyExposed))
					throw new WeavingException(String.Format("Already exposing type {0} of field {1}, error while trying to expose {2}", alreadyExposed.Member.FieldType.FullName, alreadyExposed.Member, fieldToProcess));

				ExposeMembers(typeToProcess, fieldToProcess);
				exposedTypes.Add(fieldToProcess.FieldType.FullName, fap);

				RemoveFodyAttributes(fieldToProcess);
			}

			var fieldsWithExposeInterfaceImplicitlyAttribute = typeToProcess.Fields.WithAttribute<FieldDefinition>(ExposeInterfaceImplicitlyAttribute);
			foreach (var fap in fieldsWithExposeInterfaceImplicitlyAttribute)
			{
				var fieldToProcess = fap.Member;

				MemberDefinitionCustomAttributePair<FieldDefinition> alreadyExposed;
				if (exposedTypes.TryGetValue(fieldToProcess.FieldType.FullName, out alreadyExposed))
					throw new WeavingException(String.Format("Already exposing type {0} of field {1}, error while trying to expose {2}", alreadyExposed.Member.FieldType.FullName, alreadyExposed.Member, fieldToProcess));

				ExposeInterface(typeToProcess, fieldToProcess, @explicit: false);
				exposedTypes.Add(fieldToProcess.FieldType.FullName, fap);

				RemoveFodyAttributes(fieldToProcess);
			}

			var fieldsWithExposeInterfaceExplicitlyAttribute = typeToProcess.Fields.WithAttribute<FieldDefinition>(ExposeInterfaceExplicitlyAttribute);
			foreach (var fap in fieldsWithExposeInterfaceExplicitlyAttribute)
			{
				var fieldToProcess = fap.Member;

				MemberDefinitionCustomAttributePair<FieldDefinition> alreadyExposed;
				if (exposedTypes.TryGetValue(fieldToProcess.FieldType.FullName, out alreadyExposed))
					throw new WeavingException(String.Format("Already exposing type {0} of field {1}, error while trying to expose {2}", alreadyExposed.Member.FieldType.FullName, alreadyExposed.Member, fieldToProcess));

				ExposeInterface(typeToProcess, fieldToProcess, @explicit: true);
				exposedTypes.Add(fieldToProcess.FieldType.FullName, fap);

				RemoveFodyAttributes(fieldToProcess);
			}
		}

		this.RemoveReference();
	}

	private void ExposeMembers(TypeDefinition typeToProcess, FieldReference fieldToProcess)
	{
		ExposeMembersInternal(typeToProcess, fieldToProcess, ExposeMode.None);
	}

	private void ExposeInterface(TypeDefinition typeToProcess, FieldReference fieldToProcess, bool @explicit)
	{
		var typeToExpose = fieldToProcess.FieldType;
		var resolvedTypeToExpose = typeToExpose.Resolve();

		if (!resolvedTypeToExpose.IsInterface)
			throw new WeavingException(String.Format("Type of {0}.{1} is not a interface", typeToProcess.FullName, fieldToProcess.Name));

		if (typeToProcess.Interfaces.Contains(resolvedTypeToExpose))
			throw new WeavingException(String.Format("{0} already implements interface {1}, error while trying to expose interface of {2}.{3}", typeToProcess.FullName, resolvedTypeToExpose.FullName, typeToProcess.FullName, fieldToProcess.Name));

		typeToProcess.Interfaces.Add(typeToExpose);
		ExposeMembersInternal(typeToProcess, fieldToProcess, @explicit ? ExposeMode.ImplementExplicit : ExposeMode.ImplementImplicit);
	}

	private void ExposeMembersInternal(TypeDefinition typeToProcess, FieldReference fieldToProcess, ExposeMode exposeMode)
	{
		var typeToExpose = fieldToProcess.FieldType;
		var resolvedTypeToExpose = typeToExpose.Resolve();

		foreach (var propertyToExpose in resolvedTypeToExpose.Properties.Public())
			ExposeProperty(typeToProcess, fieldToProcess, propertyToExpose, exposeMode);

		foreach (var eventToExpose in resolvedTypeToExpose.Events.Public())
			ExposeEvent(typeToProcess, fieldToProcess, eventToExpose, exposeMode);

		foreach (var methodToExpose in resolvedTypeToExpose.Methods.Public().NonSpecial())
			ExposeMethod(typeToProcess, fieldToProcess, methodToExpose, exposeMode, MethodSemanticsAttributes.None);
	}

	private PropertyDefinition ExposeProperty(TypeDefinition typeToProcess, FieldReference fieldToProcess, PropertyDefinition propertyToExpose, ExposeMode exposeMode)
	{
		var typeToExpose = fieldToProcess.FieldType;
		var genericTypeToExpose = typeToExpose as GenericInstanceType;

		var name = exposeMode == ExposeMode.ImplementExplicit ? typeToExpose.FullName + "." + propertyToExpose.Name : propertyToExpose.Name;
		var propertyToExposeType = genericTypeToExpose == null ? propertyToExpose.PropertyType : propertyToExpose.ResolveGenericPropertyType(genericTypeToExpose.GenericArguments);
		var property = new PropertyDefinition(name, PropertyAttributes.None, propertyToExposeType);

		if (propertyToExpose.GetMethod != null && propertyToExpose.GetMethod.IsPublic)
		{
			var getMethod = ExposeMethod(typeToProcess, fieldToProcess, propertyToExpose.GetMethod, exposeMode, MethodSemanticsAttributes.Getter);
			getMethod.SemanticsAttributes = MethodSemanticsAttributes.Getter;
			property.GetMethod = getMethod;
		}

		if (propertyToExpose.SetMethod != null && propertyToExpose.SetMethod.IsPublic)
		{
			var setMethod = ExposeMethod(typeToProcess, fieldToProcess, propertyToExpose.SetMethod, exposeMode, MethodSemanticsAttributes.Setter);
			setMethod.SemanticsAttributes = MethodSemanticsAttributes.Setter;
			property.SetMethod = setMethod;
		}

		AddFodyGeneratedAttributes(property);
		typeToProcess.Properties.Add(property);
		return property;
	}

	private EventDefinition ExposeEvent(TypeDefinition typeToProcess, FieldReference fieldToProcess, EventDefinition eventToExpose, ExposeMode exposeMode)
	{
		var typeToExpose = fieldToProcess.FieldType;
		var genericTypeToExpose = typeToExpose as GenericInstanceType;

		var eventToExposeType = genericTypeToExpose == null ? eventToExpose.EventType : eventToExpose.ResolveGenericEventType(genericTypeToExpose.GenericArguments);

		var name = exposeMode == ExposeMode.ImplementExplicit ? typeToExpose.FullName + "." + eventToExpose.Name : eventToExpose.Name;
		var @event = new EventDefinition(name, EventAttributes.None, eventToExposeType);

		if (eventToExpose.AddMethod != null && eventToExpose.AddMethod.IsPublic)
		{
			var addMethod = ExposeMethod(typeToProcess, fieldToProcess, eventToExpose.AddMethod, exposeMode, MethodSemanticsAttributes.AddOn);
			addMethod.SemanticsAttributes = MethodSemanticsAttributes.AddOn;
			@event.AddMethod = addMethod;
		}

		if (eventToExpose.RemoveMethod != null && eventToExpose.RemoveMethod.IsPublic)
		{
			var removeMethod = ExposeMethod(typeToProcess, fieldToProcess, eventToExpose.RemoveMethod, exposeMode, MethodSemanticsAttributes.RemoveOn);
			removeMethod.SemanticsAttributes = MethodSemanticsAttributes.RemoveOn;
			@event.RemoveMethod = removeMethod;
		}

		AddFodyGeneratedAttributes(@event);
		typeToProcess.Events.Add(@event);
		return @event;
	}

	private MethodDefinition ExposeMethod(TypeDefinition typeToProcess, FieldReference fieldToProcess, MethodReference methodToExpose, ExposeMode exposeMode, MethodSemanticsAttributes semanticAttributes)
	{
		var typeToExpose = fieldToProcess.FieldType;

		var genericTypeToExpose = typeToExpose as GenericInstanceType;
		MethodReference methodToExposeWithResolvedGenerics = null;

		if (genericTypeToExpose != null)
		{
			methodToExposeWithResolvedGenerics = methodToExpose.With(declaringType: methodToExpose.DeclaringType.MakeGenericInstanceType(genericTypeToExpose.GenericArguments.ToArray()), resolveGenericReturnTypeAndParameterTypes: true);
			methodToExpose = methodToExpose.With(declaringType: methodToExpose.DeclaringType.MakeGenericInstanceType(genericTypeToExpose.GenericArguments.ToArray()), resolveGenericReturnTypeAndParameterTypes: false);
		}
		var name = exposeMode == ExposeMode.ImplementExplicit ? typeToExpose.FullName + "." + methodToExpose.Name : methodToExpose.Name;

		MethodAttributes methodAttributes = (exposeMode == ExposeMode.ImplementExplicit ? MethodAttributes.Private : MethodAttributes.Public);
		if (exposeMode == ExposeMode.ImplementImplicit || exposeMode == ExposeMode.ImplementExplicit)
			methodAttributes |= MethodAttributes.Final;
		methodAttributes |= MethodAttributes.HideBySig;
		if (semanticAttributes != MethodSemanticsAttributes.None)
			methodAttributes |= MethodAttributes.SpecialName;
		if (exposeMode == ExposeMode.ImplementImplicit || exposeMode == ExposeMode.ImplementExplicit)
			methodAttributes |= MethodAttributes.NewSlot | MethodAttributes.Virtual;

		var method = new MethodDefinition(name, methodAttributes, (methodToExposeWithResolvedGenerics ?? methodToExpose).ReturnType);
		foreach (var genericParameter in (methodToExposeWithResolvedGenerics ?? methodToExpose).GenericParameters)
		{
			method.GenericParameters.Add(new GenericParameter(genericParameter.Name, method));
		}
		foreach (var parameter in (methodToExposeWithResolvedGenerics ?? methodToExpose).Parameters)
		{
			method.Parameters.Add(new ParameterDefinition(parameter.Name, parameter.Attributes, parameter.ParameterType));
		}

		bool methodReturnsVoid = method.ReturnType.FullName == ModuleDefinition.TypeSystem.Void.FullName;

		if (!methodReturnsVoid)
		{
			method.Body.Variables.Add(new VariableDefinition(method.ReturnType));
			method.Body.InitLocals = true;
		}

		var instructions = method.Body.Instructions;
		instructions.Add(Instruction.Create(OpCodes.Nop));
		instructions.Add(Instruction.Create(OpCodes.Ldarg_0)); // Load this
		instructions.Add(Instruction.Create(OpCodes.Ldfld,
			fieldToProcess.DeclaringType.HasGenericParameters ?
			fieldToProcess.With(declaringType: fieldToProcess.DeclaringType.MakeGenericInstanceTypeWithGenericParametersAsGenericArguments())
			: fieldToProcess)
		);

		if (methodToExpose.Parameters.Count >= 1) instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
		if (methodToExpose.Parameters.Count >= 2) instructions.Add(Instruction.Create(OpCodes.Ldarg_2));
		if (methodToExpose.Parameters.Count >= 3) instructions.Add(Instruction.Create(OpCodes.Ldarg_3));
		if (methodToExpose.Parameters.Count >= 4)
		{
			for (var i = 3; i < methodToExpose.Parameters.Count; i++)
			{
				instructions.Add(Instruction.Create(OpCodes.Ldarg_S, i + 1));
			}
		}
		instructions.Add(Instruction.Create(OpCodes.Callvirt, methodToExpose.HasGenericParameters ? methodToExpose.MakeGenericInstanceMethod(method.GenericParameters.ToArray()) : methodToExpose));
		if (methodReturnsVoid)
			instructions.Add(Instruction.Create(OpCodes.Nop));
		else
		{
			instructions.Add(Instruction.Create(OpCodes.Stloc_0));
			var inst = Instruction.Create(OpCodes.Ldloc_0);
			instructions.Add(Instruction.Create(OpCodes.Br_S, inst));
			instructions.Add(inst);
		}
		instructions.Add(Instruction.Create(OpCodes.Ret));

		if (exposeMode == ExposeMode.ImplementExplicit)
			method.Overrides.Add(methodToExpose);

		AddFodyGeneratedAttributes(method);

		typeToProcess.Methods.Add(method);
		return method;
	}

	private void AddFodyGeneratedAttributes(ICustomAttributeProvider self)
	{
		var generatedConstructor = ModuleDefinition.Import(typeof(GeneratedCodeAttribute).GetConstructor(new[] { typeof(string), typeof(string) }));
		var version = typeof(ModuleWeaver).Assembly.GetName().Version.ToString();

		var generatedAttribute = new CustomAttribute(generatedConstructor);
		generatedAttribute.ConstructorArguments.Add(new CustomAttributeArgument(ModuleDefinition.TypeSystem.String, "Expose.Fody"));
		generatedAttribute.ConstructorArguments.Add(new CustomAttributeArgument(ModuleDefinition.TypeSystem.String, version));
		self.CustomAttributes.Add(generatedAttribute);

		var debuggerConstructor = ModuleDefinition.Import(typeof(DebuggerNonUserCodeAttribute).GetConstructor(Type.EmptyTypes));
		var debuggerAttribute = new CustomAttribute(debuggerConstructor);
		self.CustomAttributes.Add(debuggerAttribute);
	}

	private void RemoveFodyAttributes(ICustomAttributeProvider self)
	{
		var customAttributes = self.CustomAttributes;

		var attribute = customAttributes.FirstOrDefault(x => x.AttributeType.Name == ModuleWeaver.ExposeMembersAttribute
			|| x.AttributeType.Name == ModuleWeaver.ExposeInterfaceImplicitlyAttribute
			|| x.AttributeType.Name == ModuleWeaver.ExposeInterfaceExplicitlyAttribute);

		if (attribute != null)
		{
			customAttributes.Remove(attribute);
		}
	}

	private void RemoveReference()
	{
		var referenceToRemove = ModuleDefinition.AssemblyReferences.FirstOrDefault(x => x.Name == "Expose");
		if (referenceToRemove != null)
		{
			ModuleDefinition.AssemblyReferences.Remove(referenceToRemove);
		}
	}
}