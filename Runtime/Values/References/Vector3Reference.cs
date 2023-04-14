using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="Vector3" />
	///     that allows you to reference a <see cref="ScriptableValue{Vector3}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class Vector3Reference : ValueReference<Vector3> { }
}
