using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="sbyte"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable S Byte Event", menuName = "Hertzole/Scriptable Values/Events/SByte Event", order = ORDER + 2)]
#endif
	public sealed class ScriptableSByteEvent : ScriptableEvent<sbyte> { }
}
