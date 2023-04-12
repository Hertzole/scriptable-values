using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="long"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Long Event", menuName = "Hertzole/Scriptable Values/Events/Long Event", order = ORDER + 7)]
#endif
	public sealed class ScriptableLongEvent : ScriptableEvent<long> { }
}
