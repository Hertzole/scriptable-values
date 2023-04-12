using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="ulong"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable U Long Event", menuName = "Hertzole/Scriptable Values/Events/ULong Event", order = ORDER + 8)]
#endif
	public sealed class ScriptableULongEvent : ScriptableEvent<ulong> { }
}
