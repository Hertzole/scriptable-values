using System;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="decimal" />
	///     that allows you to reference a <see cref="ScriptableValue{Decimal}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class DecimalReference : ValueReference<decimal> { }
}
