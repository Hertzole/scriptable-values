using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="double"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Double Event", menuName = "Aurora Punks/Scriptable Values/Events/Double Event", order = ORDER + 10)]
#endif
	public sealed class ScriptableDoubleEvent : ScriptableEvent<double> { }
}
