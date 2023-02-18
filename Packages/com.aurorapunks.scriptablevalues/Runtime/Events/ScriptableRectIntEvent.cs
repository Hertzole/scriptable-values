using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="RectInt"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Rect Int Event", menuName = "Aurora Punks/Scriptable Values/Events/RectInt Event", order = ORDER + 24)]
#endif
	public sealed class ScriptableRectIntEvent : ScriptableEvent<RectInt> { }
}
