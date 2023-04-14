using System;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="ulong" />
	///     that allows you to reference a <see cref="ScriptableValue{UInt64}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class ULongReference : ValueReference<ulong> { }
}
