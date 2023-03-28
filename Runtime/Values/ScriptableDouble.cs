using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="double"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Double", menuName = "Hertzole/Scriptable Values/Values/Double Value", order = ORDER + 9)]
#endif
	public sealed class ScriptableDouble : ScriptableValue<double> { }
}
