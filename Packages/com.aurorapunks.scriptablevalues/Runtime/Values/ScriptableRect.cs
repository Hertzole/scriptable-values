using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Rect"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Rect", menuName = "Aurora Punks/Scriptable Values/Values/Rect Value", order = ORDER + 22)]
#endif
	public sealed class ScriptableRect : ScriptableValue<Rect> { }
}
