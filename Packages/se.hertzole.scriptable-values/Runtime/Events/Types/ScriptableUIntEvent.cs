using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="uint"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable U Int Event", menuName = "Hertzole/Scriptable Values/Events/UInt Event", order = ORDER + 6)]
#endif
	public sealed class ScriptableUIntEvent : ScriptableEvent<uint> { }
}
