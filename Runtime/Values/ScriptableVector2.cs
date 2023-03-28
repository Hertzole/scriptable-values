using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Vector2"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Vector 2", menuName = "Hertzole/Scriptable Values/Values/Vector2 Value", order = ORDER + 16)]
#endif
	public sealed class ScriptableVector2 : ScriptableValue<Vector2> { }
}
