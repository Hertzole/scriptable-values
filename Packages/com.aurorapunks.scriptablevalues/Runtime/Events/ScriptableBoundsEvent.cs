using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Bounds"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Bounds Event", menuName = "Aurora Punks/Scriptable Values/Events/Bounds Event", order = ORDER + 25)]
#endif
	public sealed class ScriptableBoundsEvent : ScriptableEvent<Bounds> { }
}
