using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Rect"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Rect Event", menuName = "Hertzole/Scriptable Values/Events/Rect Event", order = ORDER + 23)]
#endif
	public sealed class ScriptableRectEvent : ScriptableEvent<Rect> { }
}
