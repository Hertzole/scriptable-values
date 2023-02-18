using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="Vector4"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Vector 4 Event", menuName = "Aurora Punks/Scriptable Values/Events/Vector4 Event", order = ORDER + 19)]
#endif
	public sealed class ScriptableVector4Event : ScriptableEvent<Vector4> { }
}
