using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="Quaternion" />
	///     that allows you to reference a <see cref="ScriptableValue{Quaternion}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class QuaternionReference : ValueReference<Quaternion> { }
}
