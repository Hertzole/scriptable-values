using System;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="char" />
	///     that allows you to reference a <see cref="ScriptableValue{Char}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class CharReference : ValueReference<char> { }
}
