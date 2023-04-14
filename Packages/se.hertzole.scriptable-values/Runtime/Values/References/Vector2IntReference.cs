using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="Vector2Int" />
	///     that allows you to reference a <see cref="ScriptableValue{Vector2Int}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class Vector2IntReference : ValueReference<Vector2Int> { }
}
