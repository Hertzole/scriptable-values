using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="ushort"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable U Short", menuName = "Aurora Punks/Scriptable Values/Values/UShort Value", order = ORDER + 3)]
#endif
	public sealed class ScriptableUShort : ScriptableValue<ushort> { }
}
