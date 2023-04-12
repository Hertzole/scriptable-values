using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="byte"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Byte Event", menuName = "Hertzole/Scriptable Values/Events/Byte Event", order = ORDER + 1)]
#endif
	public sealed class ScriptableByteEvent : ScriptableEvent<byte> { }
}
