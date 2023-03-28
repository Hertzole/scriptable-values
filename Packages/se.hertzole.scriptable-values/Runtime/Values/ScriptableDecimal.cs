using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableValue{T}" /> with a <see cref="decimal"/> value.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Decimal", menuName = "Hertzole/Scriptable Values/Values/Decimal Value", order = ORDER + 10)]
#endif
	public sealed class ScriptableDecimal : ScriptableValue<decimal> { }
}
