using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hertzole.ScriptableValues.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class NoCallbackImplementationAnalyzer : DiagnosticAnalyzer
{
	private static readonly ImmutableArray<DiagnosticDescriptor> supportedDiagnostics = ImmutableArray.Create(DiagnosticRules.NoCallbackImplementation);

	private static readonly SyntaxKind[] supportedSyntaxKinds = [SyntaxKind.Attribute];

	/// <inheritdoc />
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
	{
		get { return supportedDiagnostics; }
	}

	/// <inheritdoc />
	public override void Initialize(AnalysisContext context)
	{
		Log.Info<NoCallbackImplementationAnalyzer>("Initialized");

		context.EnableConcurrentExecution();
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

		context.RegisterSyntaxNodeAction(Action, supportedSyntaxKinds);
	}

	private static void Action(SyntaxNodeAnalysisContext context)
	{
		if (context.Node is not AttributeSyntax attribute)
		{
			return;
		}

		// Get symbol
		if (context.SemanticModel.GetSymbolInfo(attribute).Symbol is not IMethodSymbol methodSymbol)
		{
			return;
		}

		INamedTypeSymbol? attributeType = methodSymbol.ContainingType;
		if (attributeType == null)
		{
			return;
		}

		// Check if the attribute is a callback
		if (!TryGetCallbackAttribute(attributeType, out ScriptableType scriptableType))
		{
			return;
		}

		// Get member declaration syntax in parent
		MemberDeclarationSyntax? memberDeclaration = null;
		SyntaxNode? parent = attribute.Parent;
		while (memberDeclaration == null && parent != null)
		{
			if (parent is MemberDeclarationSyntax member)
			{
				memberDeclaration = member;
				break;
			}

			parent = parent.Parent;
		}

		// No member was found. Weird...
		if (memberDeclaration == null)
		{
			return;
		}

		// Get symbol for the member
		if (!memberDeclaration.TryGetFieldOrPropertyWrapper(context.SemanticModel, out ImmutableArray<FieldOrPropertyWrapper> wrappers) ||
		    wrappers.IsDefaultOrEmpty)
		{
			return;
		}

		// Get the containing type of the member
		SyntaxNode? containingTypeNode = memberDeclaration.Parent;
		if (containingTypeNode == null || context.SemanticModel.GetDeclaredSymbol(containingTypeNode) is not INamedTypeSymbol containingType)
		{
			return;
		}

		// Must have the marker attribute.
		if (!containingType.HasAttribute(Types.GLOBAL_MARKER_ATTRIBUTE))
		{
			Log.Info($"No marker attribute found on containing type {containingType}. Skipping.");
			return;
		}

		using ArrayBuilder<ITypeSymbol> parametersBuilder = new ArrayBuilder<ITypeSymbol>();

		for (int i = 0; i < wrappers.Length; i++)
		{
			parametersBuilder.Clear();

			CallbackFlags flags = CallbackFlags.None;
			FieldOrPropertyWrapper wrapper = wrappers.ItemRef(i);

			if (scriptableType == ScriptableType.Value)
			{
				ImmutableArray<AttributeData> attributes = wrapper.Symbol.GetAttributes();
				for (int j = 0; j < attributes.Length; j++)
				{
					if (methodSymbol.Equals(attributes[j].AttributeConstructor, SymbolEqualityComparer.Default))
					{
						// This is the same attribute as the one we are analyzing
						// Check if the attribute has a callback flag
						if (attributes[j].ConstructorArguments.Length > 0)
						{
							if (attributes[j].ConstructorArguments[0].Value is int callbackFlags)
							{
								if (callbackFlags == 0) // Changing
								{
									flags = CallbackFlags.PreInvoke;
								}
								else if (callbackFlags == 1)
								{
									flags = CallbackFlags.PostInvoke;
								}
							}
						}
						else
						{
							// There are no arguments. Then it's a "Changed" callback
							flags = CallbackFlags.PostInvoke;
						}
					}
				}
			}

			string callbackName = Naming.CreateCallbackName(wrapper.Name, in scriptableType, flags);

			if (!ScriptableValueHelper.TryGetScriptableType(context.SemanticModel, (INamedTypeSymbol?) wrapper.Type, out ScriptableType actualScriptableType,
				    out ITypeSymbol? genericType))
			{
				continue;
			}

			switch (actualScriptableType)
			{
				case ScriptableType.Value:
					parametersBuilder.Add(genericType!);
					parametersBuilder.Add(genericType!);
					break;
				case ScriptableType.GenericEvent:
					parametersBuilder.Add(context.Compilation.GetSpecialType(SpecialType.System_Object));
					parametersBuilder.Add(genericType!);
					break;
				case ScriptableType.Event:
					parametersBuilder.Add(context.Compilation.GetSpecialType(SpecialType.System_Object));
					parametersBuilder.Add(context.Compilation.GetTypeByMetadataName("System.EventArgs")!);
					break;
				case ScriptableType.Pool:
					parametersBuilder.Add(context.Compilation.GetTypeByMetadataName("Hertzole.ScriptableValues.PoolAction")!);
					parametersBuilder.Add(genericType!);
					break;
				case ScriptableType.List:
				case ScriptableType.Dictionary:
					parametersBuilder.Add(context.Compilation.GetCollectionChangedArgs(in genericType!));
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			ImmutableArray<ITypeSymbol> parameters = parametersBuilder.ToImmutable();

			// Check if the method is already implemented with correct parameters
			if (HasCorrectCallbackImplementation(containingType, callbackName, parameters))
			{
				continue; // Skip reporting diagnostic if method exists
			}

			ImmutableDictionary<string, string?>.Builder properties = ImmutableDictionary.CreateBuilder<string, string?>();
			properties.Add("callbackName", callbackName);
			properties.Add("fieldName", wrapper.Name);

			context.ReportDiagnostic(Diagnostic.Create(DiagnosticRules.NoCallbackImplementation, attribute.GetLocation(), properties.ToImmutableDictionary(),
				callbackName));
		}
	}

	private static bool HasCorrectCallbackImplementation(INamedTypeSymbol containingType, string callbackName, ImmutableArray<ITypeSymbol> parameters)
	{
		// Get all methods from the class with the matching name
		ImmutableArray<IMethodSymbol> candidateMethods = containingType.GetMembers(callbackName)
		                                                               .OfType<IMethodSymbol>()
		                                                               .Where(static m => m.MethodKind == MethodKind.Ordinary && m.IsPartialDefinition)
		                                                               .ToImmutableArray();

		if (!candidateMethods.Any())
		{
			return false; // No method with the correct name found
		}

		for (int i = 0; i < candidateMethods.Length; i++)
		{
			IMethodSymbol method = candidateMethods.ItemRef(i);

			if (method.PartialImplementationPart == null)
			{
				continue;
			}

			if (method.Parameters.Length != parameters.Length)
			{
				continue;
			}

			// Check if the parameters match the expected types
			bool allParametersMatch = true;
			for (int j = 0; j < method.Parameters.Length; j++)
			{
				if (!SymbolEqualityComparer.Default.Equals(method.Parameters[j].Type, parameters[j]))
				{
					allParametersMatch = false;
					break;
				}
			}

			if (allParametersMatch)
			{
				return true; // Found a matching method
			}
		}

		return false;
	}

	private static bool TryGetCallbackAttribute(INamedTypeSymbol symbol, out ScriptableType scriptableType)
	{
		string name = symbol.ToDisplayString(NullableFlowState.NotNull, SymbolDisplayFormat.FullyQualifiedFormat);

		if (string.Equals("global::Hertzole.ScriptableValues.GenerateValueCallbackAttribute", name, StringComparison.Ordinal))
		{
			scriptableType = ScriptableType.Value;
			return true;
		}

		if (string.Equals("global::Hertzole.ScriptableValues.GenerateEventCallbackAttribute", name, StringComparison.Ordinal))
		{
			scriptableType = ScriptableType.Event;
			return true;
		}

		if (string.Equals("global::Hertzole.ScriptableValues.GeneratePoolCallbackAttribute", name, StringComparison.Ordinal))
		{
			scriptableType = ScriptableType.Pool;
			return true;
		}

		if (string.Equals("global::Hertzole.ScriptableValues.GenerateCollectionCallbackAttribute", name, StringComparison.Ordinal))
		{
			// This may cause issues if it's a dictionary? 
			scriptableType = ScriptableType.List;
			return true;
		}

		scriptableType = ScriptableType.None;
		return false;
	}
}