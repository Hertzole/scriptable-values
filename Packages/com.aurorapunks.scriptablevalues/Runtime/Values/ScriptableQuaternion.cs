using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Quaternion"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Quaternion", menuName = "Aurora Punks/Scriptable Values/Values/Quaternion Value", order = ORDER + 21)]
#endif
	public sealed class ScriptableQuaternion : ScriptableValue<Quaternion> { }
}
