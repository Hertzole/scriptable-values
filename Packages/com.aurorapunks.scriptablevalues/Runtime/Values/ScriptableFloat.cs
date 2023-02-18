using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="float"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Float", menuName = "Aurora Punks/Scriptable Values/Values/Float Value", order = ORDER + 8)]
#endif
	public sealed class ScriptableFloat : ScriptableValue<float> { }
}
