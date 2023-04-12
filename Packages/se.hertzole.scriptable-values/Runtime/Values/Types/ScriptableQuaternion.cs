using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Quaternion"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Quaternion", menuName = "Hertzole/Scriptable Values/Values/Quaternion Value", order = ORDER + 21)]
#endif
	public sealed class ScriptableQuaternion : ScriptableValue<Quaternion> { }
}
