using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Vector4"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Vector 4", menuName = "Aurora Punks/Scriptable Values/Values/Vector4 Value", order = ORDER + 18)]
#endif
	public sealed class ScriptableVector4 : ScriptableValue<Vector4> { }
}
