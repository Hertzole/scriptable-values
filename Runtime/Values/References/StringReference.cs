using System;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="string" />
	///     that allows you to reference a <see cref="ScriptableValue{String}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class StringReference : ValueReference<string> { }
}
