using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Vector2"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Vector 2 Event", menuName = "Hertzole/Scriptable Values/Events/Vector2 Event", order = ORDER + 17)]
#endif
	public sealed class ScriptableVector2Event : ScriptableEvent<Vector2> { }
}
