using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="float"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Float Event", menuName = "Aurora Punks/Scriptable Values/Events/Float Event", order = ORDER + 9)]
#endif
	public sealed class ScriptableFloatEvent : ScriptableEvent<float> { }
}
