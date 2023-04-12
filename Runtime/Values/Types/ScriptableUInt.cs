using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="uint"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable U Int", menuName = "Hertzole/Scriptable Values/Values/UInt Value", order = ORDER + 5)]
#endif
	public sealed class ScriptableUInt : ScriptableValue<uint> { }
}
