using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

public readonly record struct FieldOrPropertyWrapper
{
	public string Name { get; }
	public ITypeSymbol Type { get; }
	public ISymbol Symbol { get; }

	public FieldOrPropertyWrapper(IFieldSymbol fieldSymbol)
	{
		Name = fieldSymbol.Name;
		Type = fieldSymbol.Type;
		Symbol = fieldSymbol;
	}
}