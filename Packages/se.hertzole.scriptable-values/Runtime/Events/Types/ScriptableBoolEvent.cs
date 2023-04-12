using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="bool"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Bool Event", menuName = "Hertzole/Scriptable Values/Events/Bool Event", order = ORDER + 12)]
#endif
	public sealed class ScriptableBoolEvent : ScriptableEvent<bool> { }
}
