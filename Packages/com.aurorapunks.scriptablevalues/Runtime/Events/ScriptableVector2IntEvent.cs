using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Vector2Int"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Vector 2 Int Event", menuName = "Aurora Punks/Scriptable Values/Events/Vector2Int Event", order = ORDER + 20)]
#endif
	public sealed class ScriptableVector2IntEvent : ScriptableEvent<Vector2Int> { }
}
