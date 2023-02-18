using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="bool"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Bool", menuName = "Aurora Punks/Scriptable Values/Values/Bool Value", order = ORDER + 11)]
#endif
	public sealed class ScriptableBool : ScriptableValue<bool> { }
}