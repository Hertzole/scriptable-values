using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="string"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable String Event", menuName = "Hertzole/Scriptable Values/Events/String Event", order = ORDER + 13)]
#endif
	public sealed class ScriptableStringEvent : ScriptableEvent<string> { }
}
