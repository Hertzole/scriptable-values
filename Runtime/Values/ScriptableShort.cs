using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="short"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Short", menuName = "Hertzole/Scriptable Values/Values/Short Value", order = ORDER + 2)]
#endif
	public sealed class ScriptableShort : ScriptableValue<short> { }
}
