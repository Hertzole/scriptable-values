using System;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="bool" />
	///     that allows you to reference a <see cref="ScriptableValue{Boolean}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class BoolReference : ValueReference<bool> { }
}
