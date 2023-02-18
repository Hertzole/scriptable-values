using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Vector3Int"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Vector 3 Int", menuName = "Aurora Punks/Scriptable Values/Values/Vector3Int Value", order = ORDER + 20)]
#endif
	public sealed class ScriptableVector3Int : ScriptableValue<Vector3Int> { }
}
