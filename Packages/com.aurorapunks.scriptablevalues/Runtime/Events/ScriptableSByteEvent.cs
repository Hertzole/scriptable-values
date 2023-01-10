using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="sbyte"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable SByte Event", menuName = "Aurora Punks/Scriptable Values/Events/SByte Event", order = ORDER + 2)]
#endif
	public sealed class ScriptableSByteEvent : ScriptableEvent<sbyte> { }
}