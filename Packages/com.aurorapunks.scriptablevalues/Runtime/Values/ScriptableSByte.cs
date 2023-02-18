using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="sbyte"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable S Byte", menuName = "Aurora Punks/Scriptable Values/Values/SByte Value", order = ORDER + 1)]
#endif
	public sealed class ScriptableSByte : ScriptableValue<sbyte> { }
}
