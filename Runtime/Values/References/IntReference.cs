using System;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="int" />
	///     that allows you to reference a <see cref="ScriptableValue{Int32}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class IntReference : ValueReference<int> { }
}
