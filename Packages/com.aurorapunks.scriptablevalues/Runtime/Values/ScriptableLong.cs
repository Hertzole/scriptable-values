using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a long value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Long", menuName = "Aurora Punks/Scriptable Values/Values/Long Value", order = ORDER + 6)]
#endif
	public sealed class ScriptableLong : ScriptableValue<long> { }
}