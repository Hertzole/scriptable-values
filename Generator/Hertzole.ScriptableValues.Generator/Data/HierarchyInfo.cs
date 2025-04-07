using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

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