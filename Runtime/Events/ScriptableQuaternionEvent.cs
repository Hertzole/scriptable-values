using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Quaternion"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Quaternion Event", menuName = "Hertzole/Scriptable Values/Events/Quaternion Event", order = ORDER + 22)]
#endif
	public sealed class ScriptableQuaternionEvent : ScriptableEvent<Quaternion> { }
}
