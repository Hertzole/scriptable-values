using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="Vector3Int" />
	///     that allows you to reference a <see cref="ScriptableValue{Vector3Int}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class Vector3IntReference : ValueReference<Vector3Int> { }
}
