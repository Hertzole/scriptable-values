using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Vector3"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Vector 3", menuName = "Hertzole/Scriptable Values/Values/Vector3 Value", order = ORDER + 17)]
#endif
	public sealed class ScriptableVector3 : ScriptableValue<Vector3> { }
}
