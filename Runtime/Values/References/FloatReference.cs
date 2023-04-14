using System;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="float" />
	///     that allows you to reference a <see cref="ScriptableValue{Single}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class FloatReference : ValueReference<float> { }
}
