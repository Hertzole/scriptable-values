using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="ulong"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable U Long", menuName = "Hertzole/Scriptable Values/Values/ULong Value", order = ORDER + 7)]
#endif
	public sealed class ScriptableULong : ScriptableValue<ulong> { }
}
