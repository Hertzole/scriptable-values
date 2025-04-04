using System;
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

		IncrementalValueProvider<ImmutableArray<(HierarchyInfo, CallbackData)>> values = context.SyntaxProvider
		                                                                                        .ForAttributeWithMetadataName(
			                                                                                        "Hertzole.ScriptableValues.GenerateValueCallbackAttribute",
			                                                                                        IsValidDeclaration,
			                                                                                        (syntaxContext, token) =>
				                                                                                        Get(in syntaxContext, CallbackType.Value, in token))
		                                                                                        .SelectMany(static (array, _) => array)
		                                                                                        .Where(static x => x != default)
		                                                                                        .Collect();

		IncrementalValueProvider<ImmutableArray<(HierarchyInfo, CallbackData)>> events = context.SyntaxProvider
		                                                                                        .ForAttributeWithMetadataName(
			                                                                                        "Hertzole.ScriptableValues.GenerateEventCallbackAttribute",
			                                                                                        IsValidDeclaration,
			                                                                                        (syntaxContext, token) =>
				                                                                                        Get(in syntaxContext, CallbackType.Event, in token))
		                                                                                        .SelectMany(static (array, _) => array)
		                                                                                        .Where(static x => x != default)
		                                                                                        .Collect();

		IncrementalValueProvider<ImmutableArray<(HierarchyInfo, CallbackData)>> collections = context.SyntaxProvider
		                                                                                             .ForAttributeWithMetadataName(
			                                                                                             "Hertzole.ScriptableValues.GenerateCollectionCallbackAttribute",
			                                                                                             IsValidDeclaration,
			                                                                                             (syntaxContext, token) =>
				                                                                                             Get(in syntaxContext, CallbackType.Collection,
					                                                                                             in token))
		                                                                                             .SelectMany(static (array, _) => array)
		                                                                                             .Where(static x => x != default)
		                                                                                             .Collect();

		IncrementalValueProvider<ImmutableArray<(HierarchyInfo, CallbackData)>> pools = context.SyntaxProvider
		                                                                                       .ForAttributeWithMetadataName(
			                                                                                       "Hertzole.ScriptableValues.GeneratePoolCallbackAttribute",
			                                                                                       IsValidDeclaration,
			                                                                                       (syntaxContext, token) =>
				                                                                                       Get(in syntaxContext, CallbackType.Pool, in token))
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

			CallbackFlags callbackFlags = GetCallbackFlags(in context, in callbackType, context.Attributes[i], in cancellationToken);

			builder.Add((hierarchy, new CallbackData(context.TargetSymbol.Name, callbackType, callbackFlags, memberType, scriptableType, genericType!)));
		}

		return builder.ToImmutable();
	}

	private static CallbackFlags GetCallbackFlags(in GeneratorAttributeSyntaxContext context,
		in CallbackType callbackType,
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

internal readonly record struct HierarchyInfo(string FilenameHint, string TypeName, string? Namespace, ISymbol Symbol, bool ShouldInherit)
{
	public bool IsSealed
	{
		get { return Symbol.IsSealed; }
	}

	public bool IsStruct
	{
		get { return ((ITypeSymbol) Symbol).IsValueType; }
	}

	public static HierarchyInfo FromSymbol(INamedTypeSymbol symbol)
	{
		string? nspace = null;

		if (!symbol.ContainingNamespace.IsGlobalNamespace)
		{
			nspace = symbol.ContainingNamespace.ToDisplayString();
		}

		bool shouldInherit = HasParentWithCallbacks(symbol);

		return new HierarchyInfo(symbol.GetFullyQualifiedMetadataName(), symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
			nspace, symbol, shouldInherit);
	}

	private static bool HasParentWithCallbacks(INamedTypeSymbol symbol)
	{
		if (symbol.BaseType == null)
		{
			return false;
		}

		if (symbol.BaseType.HasAttribute(Types.GLOBAL_MARKER_ATTRIBUTE))
		{
			return true;
		}

		return HasParentWithCallbacks(symbol.BaseType);
	}
}

internal readonly record struct CallbackData(
	string Name,
	CallbackType CallbackType,
	CallbackFlags Flags,
	ISymbol MemberType,
	ScriptableType ScriptableType,
	ITypeSymbol GenericType)
{
	public string MaskName { get; } = CreateMaskName(Name, in ScriptableType, in Flags);
	public string CachedFieldName { get; } = CreateCachedFieldName(Name, in ScriptableType, in Flags);

	public string RegisterCallbackMethod { get; } = CreateRegisterMethodName(true, in ScriptableType, in Flags);

	public string UnregisterCallbackMethod { get; } = CreateRegisterMethodName(false, in ScriptableType, in Flags);

	public string CallbackName { get; } = Naming.CreateCallbackName(Name, in ScriptableType, in Flags);

	public void AppendParameterTypes(in ArrayBuilder<(string name, string type)> builder)
	{
		string genericType = GenericType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

		switch (CallbackType)
		{
			case CallbackType.Value:
				builder.Add(("oldValue", genericType));
				builder.Add(("newValue", genericType));
				break;
			case CallbackType.Event:
				builder.Add(("object", "sender"));
				builder.Add(("args", ScriptableType == ScriptableType.GenericEvent ? genericType : "global::System.EventArgs"));
				break;
			case CallbackType.Collection:
				using (ArrayBuilder<char> nameBuilder = new ArrayBuilder<char>(57 + genericType.Length))
				{
					nameBuilder.AddRange("global::Hertzole.ScriptableValues.CollectionChangedArgs<");
					nameBuilder.AddRange(genericType);
					nameBuilder.Add('>');

					builder.Add(("args", nameBuilder.ToString()));
				}

				break;
			case CallbackType.Pool:
				using (ArrayBuilder<char> nameBuilder = new ArrayBuilder<char>(51 + genericType.Length))
				{
					nameBuilder.AddRange("global::Hertzole.ScriptableValues.PoolChangedArgs<");
					nameBuilder.AddRange(genericType);
					nameBuilder.Add('>');

					builder.Add(("args", nameBuilder.ToString()));
				}

				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public void AppendParameterDescriptions(in ArrayBuilder<(string name, string description)> builder)
	{
		switch (CallbackType)
		{
			case CallbackType.Value:
				builder.Add(("oldValue", DocumentationHelper.GetOldValueDescription(Flags)));
				builder.Add(("newValue", DocumentationHelper.GetNewValueDescription(Flags)));
				break;
			case CallbackType.Event:
				builder.Add(("sender", DocumentationHelper.EVENT_SENDER));
				builder.Add(("args", DocumentationHelper.EVENT_ARGS));
				break;
			case CallbackType.Collection:
				builder.Add(("args", DocumentationHelper.COLLECTION_ARGS));
				break;
			case CallbackType.Pool:
				builder.Add(("args", DocumentationHelper.POOL_ARGS));
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private static string CreateMaskName(string name, in ScriptableType scriptableType, in CallbackFlags flags)
	{
		using ArrayBuilder<char> builder = new ArrayBuilder<char>(32);

		builder.AddRange(name);
		Naming.AppendFlagSuffix(in builder, in scriptableType, in flags);

		return builder.ToString();
	}

	private static string CreateCachedFieldName(string name, in ScriptableType scriptableType, in CallbackFlags flags)
	{
		using ArrayBuilder<char> builder = new ArrayBuilder<char>(32);

		builder.AddRange(name);

		switch (scriptableType)
		{
			case ScriptableType.Value:
				builder.AddRange("ScriptableValueCallback");
				Naming.AppendFlagSuffix(in builder, in scriptableType, in flags);
				break;
			case ScriptableType.Event:
			case ScriptableType.GenericEvent:
				builder.AddRange("ScriptableEventCallback");
				break;
			case ScriptableType.List:
			case ScriptableType.Dictionary:
				builder.AddRange("ScriptableCollectionChanged");
				break;
			case ScriptableType.Pool:
				builder.AddRange("ScriptablePoolCallback");
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scriptableType), scriptableType, null);
		}

		return builder.ToString();
	}

	private static string CreateRegisterMethodName(bool subscribe, in ScriptableType scriptableType, in CallbackFlags flags)
	{
		using ArrayBuilder<char> builder = new ArrayBuilder<char>(32);

		builder.AddRange(subscribe ? "Register" : "Unregister");
		switch (scriptableType)
		{
			case ScriptableType.Value:
				builder.AddRange("Value");
				Naming.AppendFlagSuffix(in builder, in scriptableType, in flags);
				builder.AddRange("Listener");
				break;
			case ScriptableType.Event:
			case ScriptableType.GenericEvent:
				builder.AddRange("InvokedListener");
				break;
			case ScriptableType.List:
			case ScriptableType.Dictionary:
				builder.AddRange("ChangedListener");
				break;
			case ScriptableType.Pool:
				builder.AddRange("ChangedCallback");
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scriptableType), scriptableType, null);
		}

		return builder.ToString();
	}
}

internal enum CallbackType : byte
{
	Value = 0,
	Event = 1,
	Collection = 2,
	Pool = 4
}