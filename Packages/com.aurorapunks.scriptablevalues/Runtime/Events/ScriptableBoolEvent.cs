using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="bool"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Bool Event", menuName = "Aurora Punks/Scriptable Values/Events/Bool Event", order = ORDER + 12)]
#endif
	public sealed class ScriptableBoolEvent : ScriptableEvent<bool> { }
}