using System;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="byte" />
	///     that allows you to reference a <see cref="ScriptableValue{Byte}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class ByteReference : ValueReference<byte> { }
}
