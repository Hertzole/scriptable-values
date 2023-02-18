using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Color32"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Color 32 Event", menuName = "Aurora Punks/Scriptable Values/Events/Color32 Event", order = ORDER + 16)]
#endif
	public sealed class ScriptableColor32Event : ScriptableEvent<Color32> { }
}
