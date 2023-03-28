using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="float"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Float", menuName = "Hertzole/Scriptable Values/Values/Float Value", order = ORDER + 8)]
#endif
	public sealed class ScriptableFloat : ScriptableValue<float> { }
}
