using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with an <see cref="uint"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable UInt", menuName = "Aurora Punks/Scriptable Values/Values/UInt Value", order = ORDER + 5)]
#endif
	public sealed class ScriptableUInt : ScriptableValue<uint> { }
}