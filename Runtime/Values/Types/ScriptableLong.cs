using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="long"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Long", menuName = "Hertzole/Scriptable Values/Values/Long Value", order = ORDER + 6)]
#endif
	public sealed class ScriptableLong : ScriptableValue<long> { }
}
