using System;
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
}