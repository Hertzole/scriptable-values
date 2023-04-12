using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="Rect"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Rect", menuName = "Hertzole/Scriptable Values/Values/Rect Value", order = ORDER + 22)]
#endif
	public sealed class ScriptableRect : ScriptableValue<Rect> { }
}
