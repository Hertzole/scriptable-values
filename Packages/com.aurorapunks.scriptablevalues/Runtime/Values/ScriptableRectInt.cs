using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="RectInt"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Rect Int", menuName = "Aurora Punks/Scriptable Values/Values/RectInt Value", order = ORDER + 23)]
#endif
	public sealed class ScriptableRectInt : ScriptableValue<RectInt> { }
}
