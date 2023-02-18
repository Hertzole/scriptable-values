using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Color"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Color", menuName = "Aurora Punks/Scriptable Values/Values/Color Value", order = ORDER + 14)]
#endif
	public sealed class ScriptableColor : ScriptableValue<Color> { }
}
