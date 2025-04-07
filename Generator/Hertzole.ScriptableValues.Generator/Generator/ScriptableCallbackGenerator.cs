using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hertzole.ScriptableValues.Generator;

[Generator(LanguageNames.CSharp)]
public sealed partial class ScriptableCallbackGenerator : IIncrementalGenerator
{
	internal static readonly string generatorName = "Hertzole.ScriptableValues.Generator." + nameof(ScriptableCallbackGenerator);
	internal static readonly string generatorVersion = typeof(ScriptableCallbackGenerator).Assembly.GetName().Version.ToString();

	/// <inheritdoc />
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		Log.Info<ScriptableCallbackGenerator>("Initialized");

		IncrementalValueProvider<ImmutableArray<(HierarchyInfo, CallbackData)>> values =
			context.SyntaxProvider
			       .ForAttributeWithMetadataName(
				       "Hertzole.ScriptableValues.GenerateValueCallbackAttribute",
				       IsValidDeclaration,
				       (syntaxContext, token) => Get(in syntaxContext, CallbackType.Value, in token))
			       .SelectMany(static (array, _) => array)
			       .Where(static x => x != default)
			       .Collect();

		IncrementalValueProvider<ImmutableArray<(HierarchyInfo, CallbackData)>> events =
			context.SyntaxProvider
			       .ForAttributeWithMetadataName(
				       "Hertzole.ScriptableValues.GenerateEventCallbackAttribute",
				       IsValidDeclaration,
				       (syntaxContext, token) => Get(in syntaxContext, CallbackType.Event, in token))
			       .SelectMany(static (array, _) => array)
			       .Where(static x => x != default)
			       .Collect();

		IncrementalValueProvider<ImmutableArray<(HierarchyInfo, CallbackData)>> collections =
			context.SyntaxProvider
			       .ForAttributeWithMetadataName(
				       "Hertzole.ScriptableValues.GenerateCollectionCallbackAttribute",
				       IsValidDeclaration,
				       (syntaxContext, token) => Get(in syntaxContext, CallbackType.Collection, in token))
			       .SelectMany(static (array, _) => array)
			       .Where(static x => x != default)
			       .Collect();

		IncrementalValueProvider<ImmutableArray<(HierarchyInfo, CallbackData)>> pools =
			context.SyntaxProvider
			       .ForAttributeWithMetadataName(
				       "Hertzole.ScriptableValues.GeneratePoolCallbackAttribute",
				       IsValidDeclaration,
				       (syntaxContext, token) => Get(in syntaxContext, CallbackType.Pool, in token))
			       .SelectMany(static (array, _) => array)
			       .Where(static x => x != default)
			       .Collect();

		IncrementalValuesProvider<(HierarchyInfo Key, EquatableArray<CallbackData> Elements)> allCallbacks = values.Combine(events).Combine(collections)
			.Combine(pools)
			.SelectMany(static (tuple, token) =>
			{
				Dictionary<HierarchyInfo, ArrayBuilder<CallbackData>> map = new Dictionary<HierarchyInfo, ArrayBuilder<CallbackData>>();

				AddToMap(map, tuple.Left.Left.Left, token);
				AddToMap(map, tuple.Left.Left.Right, token);
				AddToMap(map, tuple.Left.Right, token);
				AddToMap(map, tuple.Right, token);

				token.ThrowIfCancellationRequested();

				ImmutableArray<(HierarchyInfo Key, EquatableArray<CallbackData> Elements)>.Builder result =
					ImmutableArray.CreateBuilder<(HierarchyInfo, EquatableArray<CallbackData>)>();

				foreach (KeyValuePair<HierarchyInfo, ArrayBuilder<CallbackData>> entry in map)
				{
					result.Add((entry.Key, entry.Value.ToImmutable()));
				}

				return result;

				static void AddToMap(Dictionary<HierarchyInfo, ArrayBuilder<CallbackData>> map,
					in ImmutableArray<(HierarchyInfo, CallbackData)> array,
					CancellationToken cancellationToken)
				{
					foreach ((HierarchyInfo key, CallbackData value) in array)
					{
						cancellationToken.ThrowIfCancellationRequested();

						if (!map.TryGetValue(key, out ArrayBuilder<CallbackData> builder))
						{
							builder = new ArrayBuilder<CallbackData>();
							map.Add(key, builder);
						}

						builder.Add(value);
					}
				}
			});

		context.RegisterImplementationSourceOutput(allCallbacks, GenerateCode);
	}

	private static bool IsValidDeclaration(SyntaxNode node, CancellationToken cancellationToken)
	{
		SyntaxNode targetNode = node;
		if (node.TryGetFieldDeclaration(out FieldDeclarationSyntax? fieldNode))
		{
			targetNode = fieldNode!;
		}
		else if (!node.IsKind(SyntaxKind.PropertyDeclaration))
		{
			return false;
		}

		cancellationToken.ThrowIfCancellationRequested();

		// Cast the node to a MemberDeclarationSyntax to check for attributes.
		MemberDeclarationSyntax memberNode = (MemberDeclarationSyntax) targetNode;

		// Must have at least one attribute.
		if (memberNode.AttributeLists.Count == 0)
		{
			return false;
		}

		cancellationToken.ThrowIfCancellationRequested();

		// We must be inside a type.
		if (targetNode.Parent is not TypeDeclarationSyntax parentTypeNode)
		{
			return false;
		}

		// Parent must have at least one attribute, usually the GenerateScriptableCallbacks attribute.
		return parentTypeNode.AttributeLists.Count > 0;
	}

	private static ImmutableArray<(HierarchyInfo, CallbackData)> Get(in GeneratorAttributeSyntaxContext context,
		in CallbackType callbackType,
		in CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		bool hasMarkerAttribute = false;

		// Make sure the containing type has the GenerateScriptableCallbacks attribute.
		ImmutableArray<AttributeData> containingTypeAttributes = context.TargetSymbol.ContainingType.GetAttributes();
		foreach (AttributeData attribute in containingTypeAttributes)
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (attribute.AttributeClass?.ToDisplayString() == "Hertzole.ScriptableValues.GenerateScriptableCallbacksAttribute")
			{
				hasMarkerAttribute = true;
				break;
			}
		}

		// Check if the marker attribute was found.
		//TODO: Use HasAttribute instead.
		if (!hasMarkerAttribute)
		{
			return ImmutableArray<(HierarchyInfo, CallbackData)>.Empty;
		}

		cancellationToken.ThrowIfCancellationRequested();

		ITypeSymbol memberType;
		switch (context.TargetSymbol)
		{
			case IFieldSymbol fieldSymbol:
				memberType = fieldSymbol.Type;
				break;
			case IPropertySymbol propertySymbol:
				memberType = propertySymbol.Type;
				break;
			default:
				return ImmutableArray<(HierarchyInfo, CallbackData)>.Empty;
		}

		// Just a safety check to make sure we have a valid type.
		if (memberType is not INamedTypeSymbol namedMemberType)
		{
			return ImmutableArray<(HierarchyInfo, CallbackData)>.Empty;
		}

		cancellationToken.ThrowIfCancellationRequested();

		// Try to get the scriptable type from the member type.
		if (!ScriptableValueHelper.TryGetScriptableType(context.SemanticModel, namedMemberType, out ScriptableType scriptableType,
			    out ITypeSymbol? genericType))
		{
			return ImmutableArray<(HierarchyInfo, CallbackData)>.Empty;
		}

		HierarchyInfo hierarchy = HierarchyInfo.FromSymbol(context.TargetSymbol.ContainingType);

		using ArrayBuilder<(HierarchyInfo, CallbackData)> builder = new ArrayBuilder<(HierarchyInfo, CallbackData)>(context.Attributes.Length);

		for (int i = 0; i < context.Attributes.Length; i++)
		{
			cancellationToken.ThrowIfCancellationRequested();

			CallbackFlags callbackFlags = GetCallbackFlags(in callbackType, context.Attributes[i], in cancellationToken);

			builder.Add((hierarchy, new CallbackData(context.TargetSymbol.Name, callbackType, callbackFlags, memberType, scriptableType, genericType!)));
		}

		return builder.ToImmutable();
	}

	private static CallbackFlags GetCallbackFlags(in CallbackType callbackType,
		in AttributeData attribute,
		in CancellationToken cancellationToken)
	{
		switch (callbackType)
		{
			case CallbackType.Value:
				return GetValueCallbackFlags(in attribute, in cancellationToken);
			default:
				return CallbackFlags.None;
		}

		static CallbackFlags GetValueCallbackFlags(in AttributeData attribute, in CancellationToken cancellationToken)
		{
			if (attribute.AttributeClass == null ||
			    attribute.AttributeClass.ToDisplayString() != "Hertzole.ScriptableValues.GenerateValueCallbackAttribute")
			{
				return CallbackFlags.None;
			}

			cancellationToken.ThrowIfCancellationRequested();

			CallbackFlags flags = CallbackFlags.None;

			bool hasChanged = false;
			bool hasChanging = false;

			// No arguments mean it's a OnXChanged
			if (attribute.ConstructorArguments.Length == 0)
			{
				hasChanged = true;
			}
			else if (attribute.ConstructorArguments.Length > 0)
			{
				// If the argument is 0, it's a OnXChanging.
				if (attribute.ConstructorArguments[0].Value is int value && value == 0)
				{
					hasChanging = true;
				}
				else
				{
					hasChanged = true;
				}
			}

			if (hasChanging)
			{
				flags |= CallbackFlags.PreInvoke;
			}

			if (hasChanged)
			{
				flags |= CallbackFlags.PostInvoke;
			}

			return flags;
		}
	}
}