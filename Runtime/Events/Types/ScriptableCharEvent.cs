using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="char"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Char Event", menuName = "Hertzole/Scriptable Values/Events/Char Event", order = ORDER + 14)]
#endif
	public sealed class ScriptableCharEvent : ScriptableEvent<char> { }
}
