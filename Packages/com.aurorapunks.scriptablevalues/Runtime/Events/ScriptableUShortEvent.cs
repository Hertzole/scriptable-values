using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="ushort"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable U Short Event", menuName = "Aurora Punks/Scriptable Values/Events/UShort Event", order = ORDER + 4)]
#endif
	public sealed class ScriptableUShortEvent : ScriptableEvent<ushort> { }
}
