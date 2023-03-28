using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Color"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Color Event", menuName = "Hertzole/Scriptable Values/Events/Color Event", order = ORDER + 15)]
#endif
	public sealed class ScriptableColorEvent : ScriptableEvent<Color> { }
}
