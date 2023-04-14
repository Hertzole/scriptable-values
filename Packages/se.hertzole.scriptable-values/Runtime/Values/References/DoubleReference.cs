using System;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="double" />
	///     that allows you to reference a <see cref="ScriptableValue{Double}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class DoubleReference : ValueReference<double> { }
}
