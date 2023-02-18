using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with an <see cref="int"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Int", menuName = "Aurora Punks/Scriptable Values/Values/Int Value", order = ORDER + 4)]
#endif
	public sealed class ScriptableInt : ScriptableValue<int> { }
}