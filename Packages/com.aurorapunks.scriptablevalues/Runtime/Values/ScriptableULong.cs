using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a ulong value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable ULong", menuName = "Aurora Punks/Scriptable Values/Values/ULong Value", order = ORDER + 7)]
#endif
	public sealed class ScriptableULong : ScriptableValue<ulong> { }
}