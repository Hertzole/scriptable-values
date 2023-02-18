using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Vector2Int"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Vector 2 Int", menuName = "Aurora Punks/Scriptable Values/Values/Vector2Int Value", order = ORDER + 19)]
#endif
	public sealed class ScriptableVector2Int : ScriptableValue<Vector2Int> { }
}
