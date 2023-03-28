using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Vector3"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Vector 3 Event", menuName = "Hertzole/Scriptable Values/Events/Vector3 Event", order = ORDER + 18)]
#endif
	public sealed class ScriptableVector3Event : ScriptableEvent<Vector3> { }
}
