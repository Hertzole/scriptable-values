using System;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="uint" />
	///     that allows you to reference a <see cref="ScriptableValue{UInt32}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class UIntReference : ValueReference<uint> { }
}
