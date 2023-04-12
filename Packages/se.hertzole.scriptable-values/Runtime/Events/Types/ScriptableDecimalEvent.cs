using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent{T}" /> with a <see cref="decimal"/> argument.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Decimal Event", menuName = "Hertzole/Scriptable Values/Events/Decimal Event", order = ORDER + 11)]
#endif
	public sealed class ScriptableDecimalEvent : ScriptableEvent<decimal> { }
}
