using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Bounds"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Bounds", menuName = "Hertzole/Scriptable Values/Values/Bounds Value", order = ORDER + 24)]
#endif
	public sealed class ScriptableBounds : ScriptableValue<Bounds> { }
}
