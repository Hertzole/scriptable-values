using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="short"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Short Event", menuName = "Hertzole/Scriptable Values/Events/Short Event", order = ORDER + 3)]
#endif
	public sealed class ScriptableShortEvent : ScriptableEvent<short> { }
}
