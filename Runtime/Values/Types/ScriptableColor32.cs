using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Color32"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Color 32", menuName = "Hertzole/Scriptable Values/Values/Color32 Value", order = ORDER + 15)]
#endif
	public sealed class ScriptableColor32 : ScriptableValue<Color32> { }
}
