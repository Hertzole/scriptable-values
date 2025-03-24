using System;
using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

public readonly record struct GenerateTypeArguments
{
	public INamedTypeSymbol ParentType { get; init; }
	public ISymbol TargetSymbol { get; init; }
	public GenerateFlags GenerateFlags { get; init; }
	public ISymbol MemberType { get; init; }

	public ReadOnlySpan<char> TargetSymbolName
	{
		get { return TargetSymbol.Name.AsSpan(); }
	}

	public void AppendCallbackName(CodeWriter writer)
	{
		writer.Append("__");
		writer.Append(TargetSymbol.Name);
		writer.Append("ScriptableValueCallback");
	}
}

[Flags]
public enum GenerateFlags
{
	None = 0,
	PreInvoke = 1,
	PostInvoke = 2
}