using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Bounds"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Bounds", menuName = "Aurora Punks/Scriptable Values/Values/Bounds Value", order = ORDER + 24)]
#endif
	public sealed class ScriptableBounds : ScriptableValue<Bounds> { }
}
