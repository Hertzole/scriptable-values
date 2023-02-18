using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="int"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Int Event", menuName = "Aurora Punks/Scriptable Values/Events/Int Event", order = ORDER + 5)]
#endif
	public sealed class ScriptableIntEvent : ScriptableEvent<int> { }
}
