using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="int"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Int", menuName = "Hertzole/Scriptable Values/Values/Int Value", order = ORDER + 4)]
#endif
	public sealed class ScriptableInt : ScriptableValue<int> { }
}
