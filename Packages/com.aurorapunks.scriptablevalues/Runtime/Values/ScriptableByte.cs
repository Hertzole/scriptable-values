using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="byte"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Byte", menuName = "Aurora Punks/Scriptable Values/Values/Byte Value", order = ORDER + 0)]
#endif
	public sealed class ScriptableByte : ScriptableValue<byte> { }
}
