using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Vector3Int"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Vector 3 Int Event", menuName = "Aurora Punks/Scriptable Values/Events/Vector3Int Event", order = ORDER + 21)]
#endif
	public sealed class ScriptableVector3IntEvent : ScriptableEvent<Vector3Int> { }
}
