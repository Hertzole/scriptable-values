using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="BoundsInt"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Bounds Int", menuName = "Hertzole/Scriptable Values/Values/BoundsInt Value", order = ORDER + 25)]
#endif
	public sealed class ScriptableBoundsInt : ScriptableValue<BoundsInt> { }
}
