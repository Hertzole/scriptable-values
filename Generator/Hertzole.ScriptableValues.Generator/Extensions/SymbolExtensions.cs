using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

internal static class SymbolExtensions
{
	public static bool StringEquals(this ISymbol symbol, string value)
	{
		return string.Equals(symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), value, StringComparison.Ordinal);
	}

	public static bool StringEquals(this ISymbol symbol, ISymbol other)
	{
		return StringEquals(symbol, other.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
	}

	public static string GetFullyQualifiedMetadataName(this ITypeSymbol symbol)
	{
		using ArrayBuilder<char> builder = new ArrayBuilder<char>();

		symbol.AppendFullyQualifiedMetadataName(in builder);

		return builder.ToString();
	}

	private static void AppendFullyQualifiedMetadataName(this ITypeSymbol symbol, in ArrayBuilder<char> builder)
	{
		BuildFrom(symbol, in builder);
		return;

		static void BuildFrom(ISymbol symbol, in ArrayBuilder<char> builder)
		{
			switch (symbol)
			{
				// Namespaces that are nested also append a leading '.'
				case INamespaceSymbol { ContainingNamespace.IsGlobalNamespace: false }:
					BuildFrom(symbol.ContainingNamespace, in builder);
					builder.Add('.');
					builder.AddRange(symbol.MetadataName.AsSpan());
					break;

				// Other namespaces (ie. the one right before global) skip the leading '.'
				case INamespaceSymbol { IsGlobalNamespace: false }:
					builder.AddRange(symbol.MetadataName.AsSpan());
					break;

				// Types with no namespace just have their metadata name directly written
				case ITypeSymbol { ContainingSymbol: INamespaceSymbol { IsGlobalNamespace: true } }:
					builder.AddRange(symbol.MetadataName.AsSpan());
					break;

				// Types with a containing non-global namespace also append a leading '.'
				case ITypeSymbol { ContainingSymbol: INamespaceSymbol namespaceSymbol }:
					BuildFrom(namespaceSymbol, in builder);
					builder.Add('.');
					builder.AddRange(symbol.MetadataName.AsSpan());
					break;

				// Nested types append a leading '+'
				case ITypeSymbol { ContainingSymbol: ITypeSymbol typeSymbol }:
					BuildFrom(typeSymbol, in builder);
					builder.Add('+');
					builder.AddRange(symbol.MetadataName.AsSpan());
					break;
			}
		}
	}

	public static bool HasAttribute(this ISymbol symbol, string fullyQualifiedAttributeName)
	{
		ImmutableArray<AttributeData> attributes = symbol.GetAttributes();
		if (attributes.IsDefaultOrEmpty)
		{
			return false;
		}

		for (int i = 0; i < attributes.Length; i++)
		{
			AttributeData attribute = attributes.ItemRef(i);

			if (attribute.AttributeClass == null)
			{
				return false;
			}

			string name = attribute.AttributeClass.ToDisplayString(NullableFlowState.NotNull, SymbolDisplayFormat.FullyQualifiedFormat);
			if (string.Equals(name, fullyQualifiedAttributeName, StringComparison.Ordinal))
			{
				return true;
			}
		}

		return false;
	}

	public static bool TryGetCallbackAttribute(this ISymbol symbol, out CallbackType callbackType)
	{
		string name;
		if (symbol is ITypeSymbol typeSymbol)
		{
			name = typeSymbol.ToDisplayString(NullableFlowState.NotNull, SymbolDisplayFormat.FullyQualifiedFormat);
		}
		else
		{
			name = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
		}

		if (name.Equals(Types.GLOBAL_GENERATE_VALUE_CALLBACK_ATTRIBUTE, StringComparison.Ordinal))
		{
			callbackType = CallbackType.Value;
			return true;
		}

		if (name.Equals(Types.GLOBAL_GENERATE_EVENT_CALLBACK_ATTRIBUTE, StringComparison.Ordinal))
		{
			callbackType = CallbackType.Event;
			return true;
		}

		if (name.Equals(Types.GLOBAL_GENERATE_POOL_CALLBACK_ATTRIBUTE, StringComparison.Ordinal))
		{
			callbackType = CallbackType.Pool;
			return true;
		}

		if (name.Equals(Types.GLOBAL_GENERATE_COLLECTION_CALLBACK_ATTRIBUTE, StringComparison.Ordinal))
		{
			callbackType = CallbackType.Collection;
			return true;
		}

		callbackType = (CallbackType) byte.MaxValue;
		return false;
	}
}